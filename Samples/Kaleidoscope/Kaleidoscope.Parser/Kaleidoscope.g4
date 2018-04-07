// ANTLR4 Grammar for the LLVM Tutorial Sample Kaleidoscope language
// To support a progression through the chapters the language features are
// selectable and dynamically adjust the parsing accordingly. This allows a
// single parser implementation for all chapters, which allows the tutorial
// to focus on the actual use of Llvm.NET itself rather than on parsing.
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

LPAREN: '(';
RPAREN: ')';
COMMA: ',';
SEMICOLON: ';';
DEF: 'def';
EXTERN: 'extern';

ASSIGN:'=';
ASTERISK: '*';
PLUS: '+';
MINUS:'-';
LEFTANGLE: '<';
SLASH: '/';

EXCLAMATION: '!';
PERCENT: '%';
AMPERSAND:'&';
PERIOD:'.';
COLON: ':';
RIGHTANGLE: '>';
QMARK: '?';
ATSIGN: '@';
BACKSLASH: '\\';
CARET: '^';
UNDERSCORE: '_';
VBAR: '|';
EQUALEQUAL: '==';
NOTEQUAL: '!=';
PLUSPLUS: '++';
MINUSMINUS: '--';

IF:     {FeatureControlFlow}? 'if';
THEN:   {FeatureControlFlow}? 'then';
ELSE:   {FeatureControlFlow}? 'else';
FOR:    {FeatureControlFlow}? 'for';
IN:     {FeatureControlFlow}? 'in';
VAR:    {FeatureMutableVars}? 'var';
UNARY:  {FeatureUserOperators}? 'unary';
BINARY: {FeatureUserOperators}? 'binary';

LineComment: '#' ~[\r\n]* EndOfLine_ -> skip;
WhiteSpace: [ \t\r\n\f]+ -> skip;

Identifier: [a-zA-Z][a-zA-Z0-9]*;
Number: Digits_ ('.' DecimalDigit_+)?;

// Parser rules ------

// built-in operator symbols
builtinop
    : ASSIGN
    | ASTERISK
    | PLUS
    | MINUS
    | SLASH
    | LEFTANGLE
    | CARET
    ;

// Allowed user defined binary symbols
userdefinedop
    : EXCLAMATION
    | PERCENT
    | AMPERSAND
    | PERIOD
    | COLON
    | RIGHTANGLE
    | QMARK
    | ATSIGN
    | BACKSLASH
    | UNDERSCORE
    | VBAR
    | EQUALEQUAL
    | NOTEQUAL
    | PLUSPLUS
    | MINUSMINUS
    ;

// unary ops can re-use built-in binop symbols (Except ASSIGN)
unaryop
    : ASTERISK
    | PLUS
    | MINUS
    | SLASH
    | LEFTANGLE
    | CARET
    | EXCLAMATION
    | PERCENT
    | AMPERSAND
    | PERIOD
    | COLON
    | RIGHTANGLE
    | QMARK
    | ATSIGN
    | BACKSLASH
    | UNDERSCORE
    | VBAR
    | EQUALEQUAL
    | NOTEQUAL
    | PLUSPLUS
    | MINUSMINUS
    ;

// All binary operators
binaryop
    : ASSIGN
    | ASTERISK
    | PLUS
    | MINUS
    | SLASH
    | LEFTANGLE
    | CARET
    | EXCLAMATION
    | PERCENT
    | AMPERSAND
    | PERIOD
    | COLON
    | RIGHTANGLE
    | QMARK
    | ATSIGN
    | BACKSLASH
    | UNDERSCORE
    | VBAR
    | EQUALEQUAL
    | NOTEQUAL
    | PLUSPLUS
    | MINUSMINUS
    ;

// pull the initializer out to a distinct rule so it is easier to get at
// the list of initializers when walking the parse tree
initializer
    : Identifier (ASSIGN expression[0])?
    ;

// Non Left recursive expressions (a.k.a. atoms)
primaryExpression
    : LPAREN expression[0] RPAREN                                                 # ParenExpression
    | Identifier LPAREN (expression[0] (COMMA expression[0])*)? RPAREN            # FunctionCallExpression
    | VAR initializer (COMMA initializer)* IN expression[0]                       # VarInExpression
    | IF expression[0] THEN expression[0] ELSE expression[0]                      # ConditionalExpression
    | FOR initializer COMMA expression[0] (COMMA expression[0])? IN expression[0] # ForExpression
    | {IsPrefixOp()}? unaryop expression[0]                                       # UnaryOpExpression
    | Identifier                                                                  # VariableExpression
    | Number                                                                      # ConstExpression
    ;

// Need to make precedence handling explicit in the code behind
// since precedence is potentially user defined at runtime.
expression[int _p]
    : primaryExpression
      ( {GetPrecedence() >= $_p}? binaryop expression[GetNextPrecedence()]
      )*
    ;

prototype
    : Identifier LPAREN (Identifier)* RPAREN                           # FunctionPrototype
    | UNARY unaryop Number? LPAREN Identifier RPAREN                   # UnaryPrototype
    | BINARY userdefinedop Number? LPAREN Identifier Identifier RPAREN # BinaryPrototype
    ;

repl
    : DEF prototype expression[0] # FunctionDefinition
    | EXTERN prototype           # ExternalDeclaration
    | expression[0]               # TopLevelExpression
    | SEMICOLON                  # TopLevelSemicolon
    ;

// Full source parse accepts a series of definitions or prototypes, all top level expressions
// are generated into a single function called Main()
fullsrc
    : repl*;
