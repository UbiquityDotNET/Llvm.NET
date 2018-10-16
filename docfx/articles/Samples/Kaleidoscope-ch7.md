# 7. Kaleidoscope: Mutable Variables
The previous chapters introduced the Kaleidoscope language and progressively implemented a variety of
language features to make a fully featured, if not simplistic, functional programming language. To a
certain extent the choice of a functional language was a bit of a cheat. Generating LLVM IR for a 
functional language is straight forward as functional languages map very easily into the LLVM native
[SSA form](http://en.wikipedia.org/wiki/Static_single_assignment_form). While the SSA form is very
useful for transformations and optimizations it is sometimes overwhelming to new users of LLVM. In
particular it may seem like LLVM doesn't support imperative languages with mutable variables or that
you need to convert all such languages into SSA form before generating LLVM IR. That is a bit of a
daunting task that might scare off a number of users. The good news is, there is no need for a language
front-end to convert to SSA form directly. In fact, it is generally discouraged! LLVM already has very
efficient, and more importantly, well tested, support for converting to SSA form (though how that works
might be a bit surprising).

## Mutable Variables in LLVM
### Mutable Variables vs. SSA, What's the big deal?
Consider the following simple "C" code:
```C
int G, H;

int test(_Bool Condition)
{
  int X;
  if (Condition)
    X = G;
  else
    X = H;
  return X;
}
```

The general idea of how to handle this in LLVM SSA form was already covered in [Chapter 5](Kaleidoscope-ch5.md).
Since there are two possible values for X when the function returns, a PHI node is inserted to merge the values.
The LLVM IR for this would look like this:

```llvm
@G = weak global i32 0   ; type of @G is i32*
@H = weak global i32 0   ; type of @H is i32*

define i32 @test(i1 %Condition) {
entry:
  br i1 %Condition, label %cond_true, label %cond_false

cond_true:
  %X.0 = load i32* @G
  br label %cond_next

cond_false:
  %X.1 = load i32* @H
  br label %cond_next

cond_next:
  %X.2 = phi i32 [ %X.1, %cond_false ], [ %X.0, %cond_true ]
  ret i32 %X.2
}
```

A full treatise on SSA is beyond the scope of this tutorial. If you are interested, there are plenty of
[resources available on-line](http://en.wikipedia.org/wiki/Static_single_assignment_form). The focus for
this chapter is on how traditional imperative language front-ends can use the LLVM support for mutable
values without performing SSA conversion up-front. While, LLVM requires IR in SSA form (there's no such
thing as "non-SSA mode"). Constructing the SSA form requires non-trivial algorithms and data structures,
so it is both wasteful and error-prone for every front-end to have to manage implementing such a thing.

### Memory in LLVM
The trick to the apparent incompatibility of SSA in LLVM IR and mutable values in imperative languages
lies in how LLVM deals with memory. While LLVM requires all register values in SSA form, it does not
require, or even permit, memory objects in SSA form. In the preceding example, access to global values G
and H are direct loads of memory. They are not named or versioned in any way. This differs from some other
compiler implementations that try to version memory objects. In LLVM, instead of encoding data-flow
analysis of memory in the IR, it is handled with Analysis Passes, which are computed on demand. This
further helps to reduce the work load of building a front-end while re-using well tested support in the
LLVM libraries.

Given all of that, the general idea is to create a stack variable, which lives in memory, for each mutable
object in a function. Since LLVM supports loads and stores from/to memory - mutable values are fairly
straight forward. Though, they may seem terribly inefficient at first. But, fear not LLVM has a way to deal
with that. (Optimizations and efficiency is getting ahead of things a bit.)

In LLVM, memory accesses are always explicit with load/store instructions. LLVM has no "address-of"
operator, and doesn't need one. Notice the type of the LLVM variables @G, and @H from the sample are
actually `i32*` even though the variable is defined as i32. In other words, @G (and @H) defines space for
an i32, but the actual symbolic name refers to the address for that space (e.g. it's a pointer). Stack
variables work the same way, except that instead of static allocation via a global declaration they are
declared with the [LLVM alloca instruction](xref:Llvm.NET.Instructions.Alloca).

```llvm
define i32 @example() {
entry:
  %X = alloca i32           ; type of %X is i32*.
  ...
  %tmp = load i32* %X       ; load the stack value %X from the stack.
  %tmp2 = add i32 %tmp, 1   ; increment it
  store i32 %tmp2, i32* %X  ; store it back
  ...
```

This code shows how LLVM supports creation and manipulation of stack based variables. Stack memory allocated
with alloca is completely generalized. you can pass the address of a stack slot to a function, store it in
a variable, etc... Using alloca, the previous example could be re-written using alloca without the PHI node
as follows:

```llvm
@G = weak global i32 0   ; type of @G is i32*
@H = weak global i32 0   ; type of @H is i32*

define i32 @test(i1 %Condition) {
entry:
  %X = alloca i32           ; type of %X is i32*.
  br i1 %Condition, label %cond_true, label %cond_false

cond_true:
  %X.0 = load i32* @G
  store i32 %X.0, i32* %X   ; Update X
  br label %cond_next

cond_false:
  %X.1 = load i32* @H
  store i32 %X.1, i32* %X   ; Update X
  br label %cond_next

cond_next:
  %X.2 = load i32* %X       ; Read X
  ret i32 %X.2
}
```

This example shows the general approach for handling arbitrary mutable values in LLVM IR without the need
for PHI nodes.

1. Mutable Variables become a stack allocation
2. Reading the variable uses a load instruction to retrieve the value from memory
3. Updates of the variable becomes a store instruction to write the value to memory
4. Taking the address of a variable just uses the stack address directly

This nicely and cleanly handles mutable variables in a fairly simple and easy to generate form. However, it
has apparently introduced a new problem. Every variable use requires stack memory and reads/writes operate
directly on stack memory - a major performance penalty. Fortunately, as previously hinted, LLVM has a well
tuned optimization pass named "mem2reg" that handles this case, promoting allocas into SSA registers, inserting
PHI nodes as necessary. For example if you run the alloca version of the IR code through the mem2reg optimization
pass you get:

```llvm
$ llvm-as < example.ll | opt -mem2reg | llvm-dis
@G = weak global i32 0
@H = weak global i32 0

define i32 @test(i1 %Condition) {
entry:
  br i1 %Condition, label %cond_true, label %cond_false

cond_true:
  %X.0 = load i32* @G
  br label %cond_next

cond_false:
  %X.1 = load i32* @H
  br label %cond_next

cond_next:
  %X.01 = phi i32 [ %X.1, %cond_false ], [ %X.0, %cond_true ]
  ret i32 %X.01
}
```

The mem2reg pass implements the standard "iterated dominance frontier" algorithm for building
the SSA form with specialized optimizations to speed up common degenerate cases. The mem2reg pass
is an integral part of the full solution to mutable variables. Using mem2reg is highly recommended.
There are a few conditions for using mem2reg correctly.

1. mem2reg is based on alloca: it looks for and promotes alloca. It does not apply to globals or heap allocations.
1. mem2reg only looks for alloca instructions in the **entry block** of the function. Placing Alloca instructions for
all variables, in all scopes, in the entry block ensures they are executed only once, which makes the conversion
simpler.
1. mem2reg only promotes Alloca instructions whose only uses are direct loads and stores. If the address of the object
is passed to a function or any pointer math applied the alloca is **not** promoted.
1. mem2reg only works on Alloca instructions of first class values (such as pointers, scalars and vectors), and only if
the array size of the allocation is 1.
1. mem2reg is not capable of promoting structs or arrays to registers. (The SROA pass is more powerful and can promote structs, unions and arrays in many cases)

These may seem onerous but are really fairly straight forward and easy to abide, the rest of this chapter
will focus on doing that with the Kaleidoscope language. If you are considering doing your own SSA consider
the following aspects of the existing LLVM patterns and mem2reg:

* The mem2reg and alloca pattern is proven and very well tested. The most common clients of LLVM use this
for the bulk of their variables, bugs are found fast and early.
* It is fast, the LLVM implementation has a number of optimizations that make it fast in common cases and
fully general. This includes fast-paths for variables used only in a single block, variables with only a
single assignment point, and heuristics to help avoid phi nodes when not needed.
* It is needed for debug info generation, debug info in LLVM relies on having the address of the variable
exposed so that debugging data is attached to it. The mem2reg+alloca pattern fits well with this debug info
style.
* It's really simple to do, letting you focus on the core of the front-end instead of the details of correctly
building SSA form.

## Generating LLVM IR for Mutable Variables
Now that we've covered the general concepts of how LLVM supports mutable variables we can focus on implementing
mutable variables in Kaleidoscope. This includes the following new features:

1. Mutate variables with an assignment operator '='
2. Ability to define new variables

Generally the first item is the primary feature here. Though, at this point, the Kaleidoscope language only
has variables for incoming arguments and for loop induction variables. Defining variables is just a generally
useful concept that can serve many purposes, including self documentation. The following is an example on
how these features are used:

```Kaleidoscope
# Define ':' for sequencing: as a low-precedence operator that ignores operands
# and just returns the RHS.
def binary : 1 (x y) y;

# Recursive fib, we could do this before.
def fib(x)
  if (x < 3) then
    1
  else
    fib(x-1)+fib(x-2);

# Iterative fib.
def fibi(x)
  var a = 1, b = 1, c in
  (for i = 3, i < x in
     c = a + b :
     a = b :
     b = c) :
  b;

# Call it.
fibi(10);
```

In order to mutate variables the current implementation needs to change to leverage the "alloca trick".
Then support for assignment will complete the mutable variables support.

## Adjusting Existing Variables for Mutation
Currently the symbol stack in Kaleidoscope stores LLVM Values directly. To support mutable values the
 NamedValues ScopeStack needs to switch to using [Alloca](xref:Llvm.NET.Instructions.Alloca).
```C#
private readonly ScopeStack<Alloca> NamedValues;
```

### Update Visitor for VariableReferenceExpression
The first change to the existing code generation is to update handling of variable expressions to generate
a load through the pointer created with an alloca instruction. This is pretty straight forward since the
scope map now stores the alloca instructions for the variable.

[!code-csharp[VisitVariableExpression](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#VariableReferenceExpression)]

### Update Visitor for ConditionalExpression
Now that we have the alloca support we can update the conditional expression handling to remove the need
for direct PHI node construction. This involves adding a new compiler generated local var for the result
of the condition and storing the result value into that location for each side of the branch. Then, in the
continue block load the value from the location so that it is available as a value for the result of the
expression.

[!code-csharp[VisitConditionalExpression](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#ConditionalExpression)]

### Update Visitor for ForInExpression
Next up is to update the for loop handling to use Alloca. The code is almost identical except for the
use of load/store for the variables and removal of the manually generated PHI nodes.

[!code-csharp[VisitForExpression](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#ForInExpression)]

### Update Visitor for FunctionDefinition
To support mutable function argument variables the handler for functions requires a small update to create
the Alloca for each incoming argument and for each of the local variables used by the function. The AST
generation tracks the variable declarations in a function so they are all available to generate directly
into the entry block.

[!code-csharp[DefineFunction](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#FunctionDefinition)]


### InitializeModuleAndPassManager
The last piece required for mutable variables support is to include the optimization pass to promote memory
to registers.

[!code-csharp[InitializeModuleAndPassManager](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#InitializeModuleAndPassManager)]

### Add operator support for Assignment Expressions
Unlike the other binary operators assignment doesn't follow the same emit left, emit right, emit operator
sequence. This is because an expression like '(x+1) = expression' is nonsensical and therefore not allowed.
The left hand side is always an alloca as the destination of a store. To handle this special case the
Generator doesn't generate for the left side, but instead looks up the Alloca for the variable. The generator
then implements a store operation of the right hand side value to the Alloca for the left side.

[!code-csharp[BinaryOperatorExpression](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#BinaryOperatorExpression)]

Now that we have mutable variables and assignment we can mutate loop variables or input parameters. For
example:

```Kaleidoscope
# Function to print a double.
extern printd(x);

# Define ':' for sequencing: as a low-precedence operator that ignores operands
# and just returns the RHS.
def binary : 1 (x y) y;

def test(x)
  printd(x) :
  x = 4 :
  printd(x);

test(123);
```

When run, this prints `1234` and `4`, showing that the value was mutated as, expected.

## User-defined Local Variables
As described in the general syntax discussion of the Kaleidoscope language [VarInExpression](Kaleidoscope-ch2.md#varinexpression)
the VarIn expression is used to declare local variables for a scope. A few changes are required to support
this language construct.

### Add Visitor for VarInExpression
The VarIn expression visitor needs to handle the mutability of the scoped variables. The basic idea for each
VarIn expression is to push a new scope on the scope stack then walk through all the variables in the
expression to define them and emit the expression for the initializer. After all the values are defined the
child expression "scope" is emitted, which may contain another VarIn or loop expression. Once the emit
completes, the variable scope is popped from the stack to restore back the previous level.

[!code-csharp[VisitVarInExpression](../../../Samples/Kaleidoscope/Chapter7/CodeGenerator.cs#VarInExpression)]

## Conclusion
This completes the updates needed to support mutable variables with potentially nested scopes. All of this
without needing to manually deal with PHI nodes or generate SSA form! Now, that's convenient!


