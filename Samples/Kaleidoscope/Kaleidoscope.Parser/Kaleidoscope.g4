// ANTLR4 Grammar for the LLVM Tutorial Sample Kaleidoscope language
// The grammar supports almost all of the chapters from the C++ tutorial
// To support a progression through the chapters the language features are
// selectable and dynamicly adjust the parsing accordingly. This allows a
// single parser implementation for all chapters, which allows the tutorial
// to focus on the actual use of LLVM itself.
//
grammar Kaleidoscope;

// Lexer Rules -------
fragment EndOfLine : '\r'? '\n';
fragment NonZeroDecimalDigit_: [1-9];
fragment DecimalDigit_: '0' | NonZeroDecimalDigit_;
fragment Digits_: DecimalDigit_+;

WhiteSpace : [ \t\r\n\f]+ -> skip;
Identifier : [a-zA-Z][a-zA-Z0-9]*;
Number: Digits_ ('.' Digits_)?;
LETTER: '\u0000'..'\u0080';


// simple line based comment
Comment : '#' ~[\r\n]* EndOfLine -> skip;

// Parser rules ------

// Identifier as a parser rule enables adaptation of keywords based on language features
// Depending on the features enabled, certain keywords may be valid identifiers.
identifier
    : Identifier
    | {!FeatureControlFlow}? 'if'
    | {!FeatureControlFlow}? 'then'
    | {!FeatureControlFlow}? 'else'
    | {!FeatureControlFlow}? 'for'
    | {!FeatureControlFlow}? 'in'
    | {!FeatureMutableVars}? 'var'
    | {!FeatureUserOperators}? 'unary'
    | {!FeatureUserOperators}? 'binary'
    ;

initializer
    : identifier ('=' expression)?
    ;

// Non Left recursive expressions
primaryExpression
    : identifier                                                                               # VariableExpression
    | Number                                                                                   # ConstExpression
    | '(' expression ')'                                                                       # ParenExPression
    | identifier '(' (expression (',' expression)*)? ')'                                       # FunctionCallExpression
    | {FeatureMutableVars}? 'var' initializer (initializer)* 'in' expression                   # VarInExpression
    | {FeatureControlFlow}? 'if' expression 'then' expression 'else' expression                # ConditionalExpression
    | {FeatureControlFlow}? 'for' initializer ',' expression (',' expression)? 'in' expression # ForExpression
    | {FeatureMutableVars}? identifier '=' expression                                          # AssignmentExpression
    | {IsPrefixOp(_input.Lt(1))}? LETTER expression                                            # PrefixOperator
    ;

// Left-recursive expressions use ANTLR to unroll the left recursion when generating the
// parser. The generated KaleidoScopeParser can then override the EnterRecursionRule and
// PrecPred methods to handle the dynamic precedence evaluation allowing this grammar to
// remain clean and simple, yet capable of handling user defined operators and precedence
expression
    : primaryExpression            # AtomExpression
    | expression LETTER expression # BinaryOpExpression
    ;

prototype
    : identifier '(' (identifier)* ')';

repl
    : 'def' prototype expression                                                                      # FunctionDefinition
    | {FeatureUserOperators}? 'def' 'unary' LETTER Number? '(' identifier ')' expression              # UnaryOpDefinition
    | {FeatureUserOperators}? 'def' 'binary' LETTER Number? '(' identifier identifier ')' expression  # BinaryOpDefinition
    | 'extern' prototype                                                                              # ExternalDeclaration
    | expression                                                                                      # TopLevelExpression
    | ';'                                                                                             # TopLevelSemicolon
    ;
