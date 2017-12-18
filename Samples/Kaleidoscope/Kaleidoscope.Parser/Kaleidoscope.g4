// ANTLR4 Grammar for the LLVM Tutorial Sample Kaleidoscope language
// To support a progression through the chapters the language features are
// selectable and dynamicly adjust the parsing accordingly. This allows a
// single parser implementation for all chapters, which allows the tutorial
// to focus on the actual use of Llvm.NET itself rather than on parsing.
//
grammar Kaleidoscope;

// virtual tokens generated from code behind
tokens { IF, THEN, ELSE, FOR, IN, VAR, UNARY, BINARY }

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

// Action attached to this may convert the identifier to a keyword token
// if it matches a keyword AND the language feature is enabled. Doing this
// as opposed to using a semantic predicate allows skipping creating a parser
// rule for an identifier.
Identifier: [a-zA-Z][a-zA-Z0-9]* {DynamciallyAdjustKeywordOrIdentifier();};
Number: Digits_ ('.' DecimalDigit_+)?;

// Parser rules ------

initializer
    : Identifier (EQUALS expression[0])?
    ;

// parser rule to handle the use of the Lexer EQUALS symbol having different meanings in multiple contexts
opsymbol
    : EQUALS
    | OPSYMBOL
    ;

// Non Left recursive expressions (a.k.a. atoms)
primaryExpression
    : Identifier                                                                  # VariableExpression
    | Number                                                                      # ConstExpression
    | LPAREN expression[0] RPAREN                                                 # ParenExpression
    | Identifier LPAREN (expression[0] (COMMA expression[0])*)? RPAREN            # FunctionCallExpression
    | VAR initializer (initializer)* IN expression[0]                             # VarInExpression
    | IF expression[0] THEN expression[0] ELSE expression[0]                      # ConditionalExpression
    | FOR initializer COMMA expression[0] (COMMA expression[0])? IN expression[0] # ForExpression
    | Identifier EQUALS expression[0]                                             # AssignmentExpression
    | {IsPrefixOp(_input.Lt(1))}? opsymbol expression[0]                          # UnaryOpExpression
    ;

// need to make precedence handling explicit in the code behind
// While some measure of functionality is achievable without an
// explicit argument and leveraging a overrides of EnterRecursionRule()
// and PrecPred(). However, that's not a fully viable option as
// ANTLR's ATN used for prediction will have the values hard
// coded.
expression[int _p]
    : primaryExpression
      ( {GetPrecedence(_input.Lt(1)) >= $_p}? op=opsymbol expression[GetNextPrecedence(_input.Lt(-1))]
      )*
    ;

prototype
    : Identifier LPAREN (Identifier)* RPAREN                      # FunctionPrototype
    | UNARY opsymbol Number? LPAREN Identifier RPAREN             # UnaryPrototype
    | BINARY opsymbol Number? LPAREN Identifier Identifier RPAREN # BinaryPrototype
    ;

repl
    : DEF prototype expression[0] # FunctionDefinition
    | EXTERN prototype            # ExternalDeclaration
    | expression[0]               # TopLevelExpression
    | SEMICOLON                   # TopLevelSemicolon
    ;
