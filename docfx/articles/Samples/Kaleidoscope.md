# 1. Kaleidoscope: Language Introduction
The general flow of this tutorial follows that of the official
[LLVM tutorial](http://releases.llvm.org/6.0.1/docs/tutorial/LangImpl01.html)
and many of the samples are lifted directly from that tutorial to make it easier to follow
along both tutorials to see how the various LLVM concepts are projected in the Llvm.NET library.

## Overview
Kaleidoscope is a simple functional language that is used to illustrate numerous real world
use cases for Llvm.NET for code generation and JIT execution.

It is worth pointing out that this example is not intended as a treatise on compiler design nor
on language parsing. While it contains many aspects of those topics the tutorial is, mostly, focused
on the use of Llvm.NET for code generation. Furthermore it isn't a trans-literation of the LLVM C++
sample as that would defeat one of the major points of Llvm.NET - to provide a familiar API and
use pattern to C# developers.

## General layout
The samples are built using common core libraries and patterns. They are explicitly designed to
make code comparisons between chapters via your favorite code comparison tool. Each, chapter builds
on the next so running a comparison makes it easy to see the changes in full context. The text of
the tutorials explains why the changes are made and a comparison helps provide the "big picture"
view.

## Variations from the Official LLVM Tutorial
The Llvm.NET version of the Kaleidoscope series takes a different route for parsing from the
LLVM implementation. In particular the Llvm.NET version defines a formal grammar using
[ANTLR4](http://antlr.org) with the full grammar for all variations of the language features in
a single assembly. Ultimately the parsing produces an AST so that the actual technology used for
the parse is hidden as an implementation detail. This helps in isolating the parsing from the use
of Llvm.NET for code generation and JIT compilation for interactive languages.

## The Kaleidoscope Language
### General Concepts
Kaleidoscope is a simple functional language with the following major features:

* Only a single data type (equivalent to C double)
* Built-in operators for basic expressions
* If/then/else style control flow
* For loop style control flow
* User defined operators
  - User defined operators can specify operator precedence
    - User defined precedence is arguably the most complex part of parsing and implementing the language.
      Though, ANTLR4's flexibility made it fairly easy to do once fully understood. (more details in
      [Chapter 6](Kaleidoscope-ch6.md))

### Expressions
In Kaleidoscope, everything is an expression (e.g. everything has or returns a value even if the value
is a constant 0.0). There are no statements and no "void" functions etc...

#### Multi-line expressions
There are a few different ways to represent an expression that is long enough to warrant splitting it across
multiple lines when typing it out.

##### Expression Continuation Marker
One mechanism for handling multi-line expressions that is used in most shell scripting languages is a line
continuation marker. In such cases a special character followed by a line-termination char or char sequence
indicates that the expression continues on the next line (e.g. it isn't complete yet).

##### Expression Complete Marker
Another approach to handling long expressions spanning multiple lines is basically the opposite of line
continuation, expression complete markers. This marker indicates the end of a potentially multi-line expression. (A variant
of this might require a line termination following the marker as with the line continuation).

##### Kaleidoscope Implementation
The original LLVM C++ implementation chose the expression completion approach using a semicolon as the completion.
(So it seems familiar like statements in other C like languages) Therefore, the LLVM.NET tutorial follows the same
design. [Implementing the line continuation mechanism in Kaleidoscope is left as an exercise for the reader :wink:]

### Example
The following example is a complete program in Kaleidoscope that will generate a textual representation
of the classic Mandelbrot Set.

```Kaleidoscope
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
>[!NOTE]
>This example uses features of the language only enabled/discussed in Chapter 6 of the tutorial.
>The runtime from chapters 3-5 will generate errors trying to parse this code.

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
Kaleidoscope is a simple yet complete language with a good deal of functionality. This serves as
a great language to study the use of LLVM for code generation. While, generally speaking, the 
Llvm.NET version of this tutorial differs only slightly from that of the official LLVM version, it
serves well as an example of what you can do with Llvm.NET.
