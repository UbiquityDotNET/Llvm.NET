// ANTLR4 Grammar for the LLVM Tutorial Sample Kaleidoscope language
// The grammar supports almost all of the chapters from the C++ tutorial
// To support a progression through the chapters the language features are
// selectable and dynamicly adjust the parsing accordingly. This allows a
// single parser implementation for all chapters, which allows the tutorial
// to focus on the actual use of LLVM itself.
//
grammar Kaleidoscope;

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

LineComment: '#' ~[\r\n]* -> skip;
WhiteSpace: [ \t\r\n\f]+ -> skip;

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

Identifier: [a-zA-Z][a-zA-Z0-9]*;
Number: Digits_ ('.' DecimalDigit_+)?;

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
// parser. The generated KaleidoScopeParser can then override the EnterRecursionRule and
// PrecPred methods to handle the dynamic precedence evaluation allowing this grammar to
// remain clean and simple, yet capable of handling user defined operators and precedence
expression
    : primaryExpression              # AtomExpression
    | expression opsymbol expression # BinaryOpExpression
    ;

prototype
    : identifier LPAREN (identifier)* RPAREN                      # FunctionProtoType
    | UNARY opsymbol Number? LPAREN identifier RPAREN             # UnaryProtoType
    | BINARY opsymbol Number? LPAREN identifier identifier RPAREN # BinaryProtoType
    ;

repl
    : DEF prototype expression # FunctionDefinition
    | EXTERN prototype         # ExternalDeclaration
    | expression               # TopLevelExpression
    | SEMICOLON                # TopLevelSemicolon
    ;
