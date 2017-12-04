# Kaleidoscope Tutorial
This series of samples generally follows the official [LLVM tutorial](http://releases.llvm.org/5.0.0/docs/tutorial/LangImpl01.html).

## Overview
Kaliedoscope is a simple functional language that is used to illustrate numerous real world
use cases for Llvm.NET for code generation and JIT execution. 

It is worth pointing out that this example isn't intended as a treatise on compiler design nor
on language parsing. While it contains aspects of those topics the sample is focused on the
use of Llvm.NET for code generation. Furthermore it isn't a trans-literation of the LLVM C++
sample as that would defeat one of the major points of Llvm.NET - to provde a familiar API and
use pattern to C# developers.

## General layout
The Llvm.NET version of the Kaleidoscope series takes a different route for parsing from the
LLVM implementation. In particular the Llvm.NET version defines a formal grammar using [ANTLR](http://antlr.org)
with the full grammar for all variations of the language features in a single assembly. This
helps in isolating the parsing from the use of Llvm.NET and minimizes the need for any sort
of custom AST. (The antlr parse tree is generally sufficient to generate code)

## The Kaleidoscope Language
### Lexer symbols

The Kaleidoscope lexer consists of several tokens and is defined in the Kaleidoscope.g4 grammar file

``` ANTLR
// Lexer Rules -------
fragment NonZeroDecimalDigit_: [1-9];
fragment DecimalDigit_: [0-9];
fragment Digits_: '0' | [1-9][0-9]*;
fragment EndOfFile_: '\u0000' | '\u001A';
fragment EndOfLine_
    : ('\r' '\n')
    | ('\r' |'\n' | '\u2028' | '\u2029')
    | EndOfFile_
    ;

EQUALS: '=';
LPAREN: '(';
RPAREN: ')';
COMMA: ',';
SEMICOLON: ';';
IF: 'if';
THEN: 'then';
ELSE: 'else';
FOR: 'for';
IN: 'in';
VAR: 'var';
UNARY: 'unary';
BINARY: 'binary';
DEF: 'def';
EXTERN: 'extern';

OPSYMBOL
    : '!'
    | '%'
    | '&'
    | '*'
    | '+'
    | '-'
    | '.'
    | '/'
    | ':'
    | '<'
    | '>'
    | '?'
    | '@'
    | '\\'
    | '^'
    | '_'
    | '|'
    ;

LineComment: '#' ~[\r\n]* EndOfLine_ -> skip;
WhiteSpace: [ \t\r\n\f]+ -> skip;

Identifier: [a-zA-Z][a-zA-Z0-9]*;
Number: Digits_ ('.' DecimalDigit_+)?;
```

This includes basic numeric patterns as well as Identifiers and the symbols allowed for operators and keywords
for the language. Subsequent chapters will introduce the meaning and use of each of these.

### Parser

The parser uses a technique of ANTLR called Semantic Predicates, that allows for dynamic adaptation of the grammar
and parser to handle variations or versions of the language. The Sample code uses that to selectively enable language
features as the chapters progress, without needing to change the grammar or generated parser code. The parser code
provides a simple means of expressing the language support level.

#### Parser grammar
``` ANTLR
// Parser rules ------

// Identifier as a parser rule enables adaptation of keywords based on language features
// Depending on the features enabled, certain keywords may be valid identifiers.
identifier
    : Identifier
    | {!FeatureControlFlow}? IF
    | {!FeatureControlFlow}? THEN
    | {!FeatureControlFlow}? ELSE
    | {!FeatureControlFlow}? FOR
    | {!FeatureControlFlow}? IN
    | {!FeatureMutableVars}? VAR
    | {!FeatureUserOperators}? UNARY
    | {!FeatureUserOperators}? BINARY
    ;

initializer
    : identifier (EQUALS expression)?
    ;

// parser rule to handle the use of the Lexer EQUALS symbol having different meanings in multiple contexts
opsymbol
    : EQUALS
    | OPSYMBOL
    ;

// Non Left recursive expressions
primaryExpression
    : identifier                                                         # VariableExpression
    | Number                                                             # ConstExpression
    | LPAREN expression RPAREN                                           # ParenExpression
    | identifier LPAREN (expression (COMMA expression)*)? RPAREN         # FunctionCallExpression
    | VAR initializer (initializer)* IN expression                       # VarInExpression
    | IF expression THEN expression ELSE expression                      # ConditionalExpression
    | FOR initializer COMMA expression (COMMA expression)? IN expression # ForExpression
    | identifier EQUALS expression                                       # AssignmentExpression
    | {IsPrefixOp(_input.Lt(1))}? opsymbol expression                    # UnaryOpExpression
    ;

// Left-recursive expressions use ANTLR to unroll the left recursion when generating the
// parser. The generated KaleidoscopeParser can then override the EnterRecursionRule() and
// PrecPred() methods to handle the dynamic precedence evaluation allowing this grammar to
// remain clean and simple, yet capable of handling user defined operators and precedence
expression
    : primaryExpression              # AtomExpression
    | expression opsymbol expression # BinaryOpExpression
    ;

prototype
    : identifier LPAREN (identifier)* RPAREN                      # FunctionPrototype
    | UNARY opsymbol Number? LPAREN identifier RPAREN             # UnaryPrototype
    | BINARY opsymbol Number? LPAREN identifier identifier RPAREN # BinaryPrototype
    ;

repl
    : DEF prototype expression # FunctionDefinition
    | EXTERN prototype         # ExternalDeclaration
    | expression               # TopLevelExpression
    | SEMICOLON                # TopLevelSemicolon
;
```

A full tutorial on ANTLR is beyond the scope of this tutorial but the basics should be familiar
enough to anyone acquainted with EBNF form to make enough sense out of it. Don't worry too much
about the details at this point as subsequent chapters will cover salient points as new features
are enabled.

