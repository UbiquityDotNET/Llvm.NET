---
uid: Kaleidoscope-ch5
---

# 5. Kaleidoscope: Control Flow
This chapter focuses on adding the support necessary to implement the if-then-else and for loop control
flow support in the Kaleidoscope language. Without some sort of control flow the Kaleidoscope language
is not particularly useful. So, this chapter completes the core language support to make it a usable
language.

## if-then-else
It is worth re-visiting the discussion of the intended syntax and semantics for conditional flow in
[Chapter 2](xref:Kaleidoscope-ch2#conditionalexpression). This will help in understanding the language
functionality to implement.

The ultimate goal of the changes to support code generation for control flow constructs is to transform
Kaleidoscope code such as:

```Kaleidoscope
extern foo();
extern bar();
def baz(x) if x then foo() else bar();
```

and generate LLVM like this (unoptimized):
```llvm
declare double @foo()

declare double @bar()

define double @baz(double %x) {
entry:
  %ifcond = fcmp one double %x, 0.000000e+00
  br i1 %ifcond, label %then, label %else

then:       ; preds = %entry
  %calltmp = call double @foo()
  br label %ifcont

else:       ; preds = %entry
  %calltmp1 = call double @bar()
  br label %ifcont

ifcont:     ; preds = %else, %then
  %iftmp = phi double [ %calltmp, %then ], [ %calltmp1, %else ]
  ret double %iftmp
}
```

The entry code will convert the input x into an llvm i1 value to use as the condition for a branch. This
is done by comparing the input value of x to 0.0 to get the condition boolean value. Then the condition is
used to branch to either the 'then' block or the 'else' block. The two target blocks contain the generated
code for the expressions for each part of the conditional and a final branch to a continuation block.

Since the code branch could flow into the continuation block from either the 'else' or 'end' blocks a phi
instruction is placed at the beginning of the continuation block with appropriate values for the result
from each of the two predecessor blocks. The resulting value is then provided as the return of the function.
It is important to note that using the phi node in this fashion does not require generating all of the code
in SSA form. In fact, doing that in the front end is strongly discouraged. Generally speaking there are
only two reasons where a phi node may crop up:

 1. Mutable variables like x = 1; x = x + 1;
 2. Values that are part of the structure of the language (usually for control flow)

[Chapter 7](xref:Kaleidoscope-ch7) Covers the mutable variables case in detail and the techniques for
generating the code without using a phi node. For cases like this one where it is straight forward and easy
to insert the phi node directly then there's no reason not to. Though, the solution provided in Chapter 7 can,
and does, eliminate the need to manually insert the phi node here as well.

### Code Generation
Generating the code for the condition expression follows the pattern shown above with the following high
level steps:

1. Generate the code for the condition value expression
2. Emit conversion of the result of the condition to an LLVM i1 by comparing to 0.0
3. Create a block for the then expression
4. Create a block for the else expression
5. Create a block for the if continuation
6. Emit conditional branch to the then, else blocks
7. Switch to the then expression block
8. Emit code for the then expression
9. Capture the insertion block location as generating the then expression may add new blocks
10. Emit a branch to the if continuation block
11. Switch to the else block
12. Emit code for the else expression
13. Emit a branch to the if continuation block
14. Capture the insertion block location as generating the else expression may add new blocks
15. Switch to the if continuation block
16. Emit phi node with the results of the insertion blocks and result values captured after generating
each of the sub expressions
17. Use the result...

That's a bit more complex than the other language constructs seen so far, but is still pretty straight
forward once you get the general gist of how LLVM IR works. There's one extra trick repeated in steps 9
and again in 14, where after generating the IR for the sub expressions, the current block insertion point
is captured. This is needed as the generation for the sub expression may include another conditional
expression, which may contain a conditional sub expression, ... Thus, the 'current block' may well have
changed from the starting block. The phi node needs the immediate predecessor block and the value it
produced, so the current block is captured after generation, before switching the block to the next one
for generation to ensure that the correct block is used with the value.

The actual code follows the description pretty closely and should now be fairly easy to follow:

[!code-csharp[ConditionalExpression](CodeGenerator.cs#ConditionalExpression)]

## For Loop
Now that the basics of control flow are available it is possible to leverage the same concepts to
implement the for loop constructs for the language.

The general idea is to transform the loops in Kaleidoscope such as this:

```Kaleidoscope
extern putchard(char);
def printstar(n)
  for i = 1, i < n, 1.0 in
    putchard(42);  # ascii 42 = '*'

# print 100 '*' characters
printstar(100);
```

In LLVM IR (unoptimized) that should look like this:

``` llvm
declare double @putchard(double)

define double @printstar(double %n) {
entry:
  ; initial value = 1.0 (inlined into phi)
  br label %loop

loop:       ; preds = %loop, %entry
  %i = phi double [ 1.000000e+00, %entry ], [ %nextvar, %loop ]
  ; body
  %calltmp = call double @putchard(double 4.200000e+01)
  ; increment
  %nextvar = fadd double %i, 1.000000e+00

  ; termination test
  %cmptmp = fcmp ult double %i, %n
  %booltmp = uitofp i1 %cmptmp to double
  %loopcond = fcmp one double %booltmp, 0.000000e+00
  br i1 %loopcond, label %loop, label %afterloop

afterloop:      ; preds = %loop
  ; loop always returns 0.0
  ret double 0.000000e+00
}
```

Thus, the basic pattern to generate the for loop code consists of the following steps:

1. Create block for loop header
2. Switch to the loop header block
2. Emit code to Initialize start value with starting value from initialization expression
4. Create block for the loop body
5. Create block for the loop end
3. Emit unconditional branch to the loop body
4. Switch to the loop body block
7. Emit phi node for the loop value with the loop header block and initial value as first predecessor
9. Push a new scope for named values as the loop body represents a new scope
10. Add the variable for the loop to the current scope
11. Emit the body expression, which may create new blocks
12. Emit the code to compute the next value (e.g. next = current + step )
13. Emit code for the end condition
14. Emit code to convert the result of the condition to an LLVM i1 for a conditional branch
15. Capture loop end block for PHI node
16. Create after loop block
17. Emit conditional branch to the loop body block or after loop block depending on the result of the end
condition
18. Add an incoming predecessor to the phi node at the beginning of the loop body for the next loop value
and the loop end block it comes from
19. Switch to after block
20. Create constant value of 0.0 as the result expression of the for loop

That's a few more steps than even the if-then-else but the basic concepts of blocks, conditional branches
and direct phi-nodes remains the same.

The code to generate a for loop follows this pattern pretty closely.

[!code-csharp[Main](CodeGenerator.cs#ForInExpression)]

The only new functionality in that is the use of the ScopeStack class to support nested scopes and the named
variables within them. ScopeStack is provided in the
Ubiquity.NET.Runtime.Utils library. It is basically a stack of name to value mapping dictionaries. The EnterScope
method will push a new dictionary on to the stack and return an IDisposable that will handle popping it back off.
This allows for nested expressions to use variables in the parent scope and to override them with its own value too.
That, is the symbols available in a loop include the loop variable and any variables in the parent scope, all the
way back to the function parameters. The stack nature allows for deeper scopes to shadow the variable of the same
name in the parent, while allowing access to all other variables from other scopes.

## Conclusion
Control flow is certainly more complex to generate than any of the other language constructs but it relies on
a few basic primitive building block patterns. Thus, it is fairly easy to understand and implement once the
basic patterns are understood. With the inclusion of control flow the Kaleidoscope language is now a complete,
albeit simplistic, functional language.
