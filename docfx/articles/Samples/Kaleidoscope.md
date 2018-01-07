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
### General Concepts
Kaleidoscope is a simple functional language with the following major features:

* Only a single data type (equivalent to C double)
* Built-in operators for basic expressions
* If/then/else style control flow
* For loop style control flow
* User defined operators
  - User defined operators can specify operator precedence
    - This is arguably the most complex part of implementing the language. Though, ANTLR4's
      flexibility made it fairly easy to do once fully understood. (more details in [Chapter 6](Kaleidoscope-ch6.md))

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
>The runtime from earlier chapters will generate errors trying to parse this code.

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
