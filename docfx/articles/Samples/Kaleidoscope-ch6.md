# 6. Kaleidoscope: User Defined Operators
At this point in the progression of the tutorial, Kaleidoscope is a fully functional, albeit fairly minimal,
language. Thus far, the tutorial has avoided details of the parsing. One of the benefits of using a tool
like ANTLR4 is that you can accomplish a lot without needing to spend a lot of time thinking about the
parser too much. With user defined operators we'll break that and get down and dirty with the parser a bit
to make the operators work. 

> [!TIP]
> The actual value of user defined operator precedence in a language is a bit debatable, and the
> initial plan for the Llvm.NET tutorials was to skip this chapter as it doesn't involve any new
> LLVM IR or code generation. After the code was done to get the other chapters working - this one
> was still nagging, begging really, for a solution. The challenge to come up with a good solution
> was ultimately too tempting to resist, and we now have a full implementation with a few useful
> extensions on top! (Exponent operator '^', '=' vs '==', '++', and '--')

## General idea of user defined operators
User defined operators in Kaleidoscope are a bit unique. Unlike C++ and other similar languages the
precedence of the user defined operators are not fixed. Though, the built-in operators all use a fixed
precedence. That poses some interesting challenges for a parser as it must dynamically adapt to the state
of the language runtime as it is parsing so that it can correctly evaluate the operator expressions.
Making that work while using ANTLR requires looking under the hood to how ANTLR4 ordinarily handles
precedence. A full treatise on the subject is outside the scope of this tutorial, but the
[ANTLR GitHub site](https://github.com/antlr/antlr4/blob/master/doc/left-recursion.md)
has a good description of the details of the precedence climbing approach used in ANTLR. The general idea
is that the expression rule takes an additional precedence argument and the operator expressions include
a semantic predicate that tests the current precedence level. If the current level is less than or equal
to the current level then that operator rule expression is allowed to match the input. Otherwise, the rule
is skipped. Usually this is all hidden by the implicit support for precedence climbing and left recursion
that is built-in to ANTLR4. However that requires fixing the precedence for operators in the grammar.
Thus, Kaleidoscope doesn't use the default left-recursion support, but does use the same concepts with a
custom hook in the code behind.

```antlr
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
```

Two custom functions are used to handle the dynamic runtime defined nature of the precedence.
1. GetPrecedence() used in the semantic predicate determines the precedence of the operator for
the current rule
2. GetNextPrecedence() is used to determine the next higher level of precedence for any child expressions

These are implemented in the partial extension of the parser:
```C#
private int GetPrecedence( )
{
    return GlobalState.GetPrecedence( _input.Lt( 1 ).Type );
}

private int GetNextPrecedence( )
{
    return GlobalState.GetNextPrecedence( _input.Lt( -1 ).Type );
}
```

These two functions use the current input state to identify the actual operator token. Get Precedence does
a look-ahead by one token to determine what the precedence for the operator is. The rest of the rule is
only executed if the precedence is greater than or equal to the current precedence. The right hand side
matches expressions of a higher precedence by doing a look-behind one token to get the next precedence
level. The custom parser functions are pretty small since they defer the real work to the GlobalState
instance provided when constructing the parser. The state is an instance of the DynamicRuntimeState class.
Up until now this state was only used to determine the language features to enable. With dynamic precedence
for user operators the state maintains a pair of tables of operator information that includes the symbol
for the operator and precedence:

```C#
private OperatorInfoCollection UnaryOps = new OperatorInfoCollection( );

private OperatorInfoCollection BinOpPrecedence = new OperatorInfoCollection( )
{
    new OperatorInfo( LEFTANGLE, OperatorKind.InfixLeftAssociative, 10, true),
    new OperatorInfo( PLUS,      OperatorKind.InfixLeftAssociative, 20, true),
    new OperatorInfo( MINUS,     OperatorKind.InfixLeftAssociative, 20, true),
    new OperatorInfo( ASTERISK,  OperatorKind.InfixLeftAssociative, 40, true),
    new OperatorInfo( SLASH,     OperatorKind.InfixLeftAssociative, 40, true),
    new OperatorInfo( CARET,     OperatorKind.InfixRightAssociative, 50, true),
    new OperatorInfo( ASSIGN,    OperatorKind.InfixRightAssociative, 2, true),
};
```

The tables are used to determine the precedence for an operator and what the next precedence should be.
They start out with the built-in binary operators. (Kaleidoscope doesn't define any unary operators so
that table starts empty) The GetPrededence() and GetNextPrecedence() functions lookup the operators token
in the table to determine the operators associativity and its precedence.

```C#
public OperatorInfo GetBinOperatorInfo( int tokenType )
{
    if( BinOpPrecedence.TryGetValue( tokenType, out var value ) )
    {
        return value;
    }

    return default;
}

public OperatorInfo GetUnaryOperatorInfo( int tokenType )
{
    if( UnaryOps.TryGetValue( tokenType, out var value ) )
    {
        return value;
    }

    return default;
}

internal int GetPrecedence( int tokenType ) => GetBinOperatorInfo( tokenType ).Precedence;

internal int GetNextPrecedence( int tokenType )
{
    var operatorInfo = GetBinOperatorInfo( tokenType );
    int retVal = operatorInfo.Precedence;
    if( operatorInfo.Kind == OperatorKind.InfixRightAssociative || operatorInfo.Kind == OperatorKind.PreFix )
    {
        return retVal;
    }

    return retVal + 1;
}
```

This provides the core ability for looking up and handling precedence. Though, as shown so far, it is just
a rather convoluted form of what ANTLR4 gives us for free. The real point of this runtime state is the
ability of the language to dynamically add user operators. By adding operators to the runtime state the
lookup process will include them during parsing. Thus, whenever visiting an operator definition it is generated
as a normal function with a specialized name and the operator and precedence are added to the runtime state.
Any future use of the operator in an expression will detect the correct precedence and the code generation
treats it as a function call with the appropriate left and right hand argument values.

Actually adding the operators to the table is handled in the parsing process itself using a feature of the
ANTLR generated parser called a "Parse Listener". A parse listener is attached to the parser and effectively
monitors the entire parsing process. For the user operators the listener will listen for the specific case
of a complete function definition that defines a user operator. When it detects such a case it will update
the runtime table accordingly.

[!code-csharp[UserOperatorListener](../../../Samples/Kaleidoscope/Kaleidoscope.Runtime/KaleidoscopeUserOperatorListener.cs)]

With the use of the listener the dynamic precedence is contained entirely in the parser. When the parse tree is
processed to produce the AST the user defined operators are transformed to simple function declarations and
function calls. This simplification allows the generator and later stages to remain blissfully ignorant of the
issue of precedence and even the existence of user defined operators.

#### AST
When building the AST Prototypes for user defined operators are transformed to a FunctionDeclaration
[!code-csharp[UserOperatorPrototypes](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/AST/AstBuilder.cs#UserOperatorPrototypes)]

During construction of the AST all occurances of a user defined operator expression are transformed into a function call for the
function that actually implements the behavior for the operator.

[!code-csharp[UserBinaryOpExpression](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/AST/AstBuilder.cs#UserBinaryOpExpression)]
[!code-csharp[UnaryOpExpression](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/AST/AstBuilder.cs#UnaryOpExpression)]

### CodeGen and Driver
If you compare the code generation and driver code between Chapter 5 and Chapter 6 you'll see the only difference is the
language level setting, it got a bump (Literally a single enum on one line of each component). Everything else is identical.
This is because the real work is on the parser and AST not the code generation. This is where having a good parser + AST model
can help keep the code generation simpler. If the parse tree alone was used, then the code generation would need additional code
similar to what is found in the AST generation. Putting it into the AST generation keeps things much cleaner as, obviously, the
support for user defined operators and precedence has nothing to do with code generation. Keeping the code generation simpler is
generally a really good thing!

That completes the support for user defined operators.

### Example
The following example is a complete program in Kaleidoscope that will generate a textual representation
of the classic Mandelbrot Set using all of the features of the language.

```kaleidoscope
def unary!(v)
  if v then
    0
  else
    1;

def unary-(v)
  0-v;

def binary> 10 (LHS RHS)
  RHS < LHS;

def binary| 5 (LHS RHS)
  if LHS then
    1
  else if RHS then
    1
  else
    0;

def binary& 6 (LHS RHS)
  if !LHS then
    0
  else
    !!RHS;

def binary= 9 (LHS RHS)
  !(LHS < RHS | LHS > RHS);

def binary : 1 (x y) y;

extern putchard(char);

def printdensity(d)
  if d > 8 then
    putchard(32)
  else if d > 4 then
    putchard(46)
  else if d > 2 then
    putchard(43)
  else
    putchard(42);

def mandelconverger(real imag iters creal cimag)
  if iters > 255 | (real*real + imag*imag > 4) then
    iters
  else
    mandelconverger( real*real - imag*imag + creal
                   , 2*real*imag + cimag
                   , iters+1
                   , creal
                   , cimag
                   );

def mandelconverge(real imag)
  mandelconverger(real, imag, 0, real, imag);

def mandelhelp(xmin xmax xstep   ymin ymax ystep)
  for y = ymin, y < ymax, ystep in
  (
    (for x = xmin, x < xmax, xstep in printdensity(mandelconverge(x,y))) : putchard(10)
  );

def mandel(realstart imagstart realmag imagmag)
  mandelhelp(realstart, realstart+realmag*78, realmag, imagstart, imagstart+imagmag*40, imagmag);

mandel(-2.3, -1.3, 0.05, 0.07);
```

When entered ( or copy/pasted) to the command line Kaleidoscope will print out the following:

```shell
Ready>mandel(-2.3, -1.3, 0.05, 0.07);
*******************************************************************************
*******************************************************************************
****************************************++++++*********************************
************************************+++++...++++++*****************************
*********************************++++++++.. ...+++++***************************
*******************************++++++++++..   ..+++++**************************
******************************++++++++++.     ..++++++*************************
****************************+++++++++....      ..++++++************************
**************************++++++++.......      .....++++***********************
*************************++++++++.   .            ... .++**********************
***********************++++++++...                     ++**********************
*********************+++++++++....                    .+++*********************
******************+++..+++++....                      ..+++********************
**************++++++. ..........                        +++********************
***********++++++++..        ..                         .++********************
*********++++++++++...                                 .++++*******************
********++++++++++..                                   .++++*******************
*******++++++.....                                    ..++++*******************
*******+........                                     ...++++*******************
*******+... ....                                     ...++++*******************
*******+++++......                                    ..++++*******************
*******++++++++++...                                   .++++*******************
*********++++++++++...                                  ++++*******************
**********+++++++++..        ..                        ..++********************
*************++++++.. ..........                        +++********************
******************+++...+++.....                      ..+++********************
*********************+++++++++....                    ..++*********************
***********************++++++++...                     +++*********************
*************************+++++++..   .            ... .++**********************
**************************++++++++.......      ......+++***********************
****************************+++++++++....      ..++++++************************
*****************************++++++++++..     ..++++++*************************
*******************************++++++++++..  ...+++++**************************
*********************************++++++++.. ...+++++***************************
***********************************++++++....+++++*****************************
***************************************++++++++********************************
*******************************************************************************
*******************************************************************************
*******************************************************************************
*******************************************************************************
*******************************************************************************
Evaluated to 0
Ready>
```

## Conclusion
Adding user defined operators with user defined precedence is fairly straight forward to implement in
terms of the code generation. No new code generation is required. ANTLR4 has support for left-recursion
in the grammar and precedence of expressions. Even though ANTLR only directly supports fixed precedence it is
rather easy to extend the underlying support to handle dynamic precedence and associativity, once the underlying
mechanics are understood. The rest is on the Ast construction as it converts the user defined operators to
function definitions and function calls. 

If you compare the code generation and driver code between Chapter 5 and Chapter 6 you'll see the only difference is the
language level setting, it got a bump (Literally a single enum on one line of each component). Everything else is identical.
This is because the real work is on the parser and AST not the code generation. This is where having a good parser + AST model
can help keep the code generation simpler. If the parse tree alone was used, then the code generation would need additional code
similar to what is found in the AST generation. Putting it into the AST generation keeps things much cleaner as, obviously, the
support for user defined operators and precedence has nothing to do with code generation. Keeping the code generation simpler is
generally a really good thing! 

>[!TIP]
>An early version of these samples skipped the use of an AST and used the parse tree directly. You can compare the history of
> the generators for that transition to see how the AST helps simplify the code generation. (Not to mention sets the stage for
> an otherwise un-implemented feature - truly lazy compilation.)



