---
uid: Kaleidoscope-ch1
---


# 1. Kaleidoscope: Language Introduction
The general flow of this tutorial follows that of the official
[LLVM tutorial](xref:llvm_kaleidoscope_tutorial)
and many of the samples are lifted directly from that tutorial to make it easier to
follow along both tutorials to see how the various LLVM concepts are projected in the
`Ubiquity.NET.Llvm library.`

>[!NOTE]
> The samples are all setup to include `<PublishAot>True</PublishAot>` and therefore
> support AOT code generation. To use that you only need to run
> `dotnet publish <project path> -r <RID>` to build the native standalone version
> of the app. This demonstrates that the libraries are AOT compatible. While this makes
> things run faster as no JIT is used, everything is already native code, it has the
> drawback of making the app RID specific. That is, you must AOT build for EVERY
> supported RID target. Each usage case must make a choice and there is no single
> "one size fits all" answer. Thus, the samples and the library itself allow for, but
> ***Do NOT*** require AOT builds.

## Overview
Kaleidoscope is a simple functional language that is used to illustrate numerous real
world use cases for Ubiquity.NET.Llvm for code generation and JIT execution.

It is worth pointing out that this example is not intended as a treatise on compiler
design nor on language parsing. While it contains many aspects of those topics the
tutorial is, mostly, focused on the use of Ubiquity.NET.Llvm for code generation.
Furthermore it isn't a trans-literation of the LLVM C++ sample as that would defeat
one of the major points of `Ubiquity.NET.Llvm` - to provide a familiar API and use
patterns familiar to C# developers.

## General layout
The samples are built using common core libraries and patterns. They are explicitly
designed to make code comparisons between chapters via your favorite code comparison
tool. Each, chapter builds on the next so running a comparison makes it easy to see
the changes in full context. The text of the tutorials explains why the changes are
made and a comparison helps provide the "big picture" view.

## Variations from the Official LLVM Tutorial
The Ubiquity.NET.Llvm version of the Kaleidoscope series takes a different route for
parsing from the LLVM implementation. In particular the Ubiquity.NET.Llvm version
defines a formal grammar using [ANTLR4](http://antlr.org) with the full grammar for
all variations of the language features in a single assembly. Ultimately the parsing
produces an [AST](xref:Kaleidoscope-AST) so that the actual technology used for the
parse is hidden as an implementation detail. This helps in isolating the parsing from
the use of Ubiquity.NET.Llvm for code generation and JIT compilation for interactive
languages.

## The Kaleidoscope Language
### General Concepts
Kaleidoscope is a simple functional language with the following major features:

* Only a single data type (equivalent to C double)
* Built-in operators for basic expressions
* If/then/else style control flow
* For loop style control flow
* User defined operators
  - User defined operators can specify operator precedence
    - User defined precedence is arguably the most complex part of parsing and
      implementing the language. Though, ANTLR4's flexibility made it fairly easy to
      do once fully understood. (more details in [Chapter 6](xref:Kaleidoscope-ch6))

### Expressions
In Kaleidoscope, everything is an expression (e.g. everything has or returns a value
even if the value is a constant 0.0). There are no statements and no "void" functions
etc...

#### Multi-line expressions
There are a few different ways to represent an expression that is long enough to
warrant splitting it across multiple lines when typing it out.

##### Expression Continuation Marker
One mechanism for handling multi-line expressions that is used in most shell
scripting languages is a line continuation marker. In such cases a special character
followed by a line-termination char or char sequence indicates that the expression
continues on the next line (e.g. it isn't complete yet).

##### Expression Complete Marker
Another approach to handling long expressions spanning multiple lines is basically
the opposite of line continuation, expression complete markers. This marker indicates
the end of a potentially multi-line expression. (A variant of this might require a
line termination following the marker as with the line continuation).

##### Kaleidoscope Implementation
The original LLVM C++ implementation chose the expression completion approach using
a semicolon as the completion. (So it seems familiar like statements in other C like
languages) Therefore, the Ubiquity.NET.Llvm tutorial follows the same design.
[Implementing the line continuation mechanism in Kaleidoscope is left as an exercise
for the reader - though if you come up with a mechanism to support either that is
determined by the calling application; PRs are welcome! :wink:]

### Example
The following example is a complete program in Kaleidoscope that will generate a
textual representation of the classic Mandelbrot Set.

[!code-Kaleidoscope[mandel.kls](mandel.kls)]

When entered ( or copy/pasted) to the command line Kaleidoscope will print out the
following:

>[!NOTE]
>This example uses features of the language only enabled/discussed in Chapter 6 of
>the tutorial.The runtime from chapters 3-5 will generate errors trying to parse this
>code.

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
Kaleidoscope is a simple language with a good deal of functionality. This serves as
a great language to study the use of Ubiquity.NET.Llvm for code generation and
Domain Specific Languages. While, generally speaking, the functionality of the
`Ubiquity.NET.Llvm` version of this tutorial differs only slightly from that of the
official LLVM version, it serves well as an example of what you can do with
`Ubiquity.NET.Llvm.`
