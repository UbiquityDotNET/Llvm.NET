---
uid: Kaleidoscope-AST
---

# Kaleidoscope Abstract Syntax Tree
As with many language parsing systems Kaleidoscope leverages an Abstract Syntax Tree (AST) to simplify
generating code from the parsed language. Each type of node in the tree implements the IAstNode interface

This interface provides the basic properties of any node in the tree for common uses. The Kaleidoscope
language is a simple one and, therefore, has only a few kinds of nodes. The AST consist of the following
basic categories of nodes:
 * [AST Node](#ast-node)
    * [Function Declaration](#function-declaration)
    * [Function Definition](#function-definition)
    * [Variable Declaration](#variable-declaration)
      * [Local Variable](#local-variable)
      * [Function Parameter](#function-parameter)
    * [Expression](#expression)
      * [Variable Reference](#variable-reference)
      * [Unary Operator](#unary-operator)
      * [Binary Operator](#binary-operator)
         - [Assignment](#assignment)
      * [Function Call](#function-call)
      * [Conditional](#conditional)
      * [For-In](#for-in)
      * [Var-In](#var-in)

## AST Node
The IAstNode interface forms the common interface for all AST nodes, it provides the common properties
for all nodes.

[!code-csharp[IAstNode](IAstNode.cs)]

## Function Definition
FunctionDefinition, as the name implies, contains the definition of a function. This includes the signature
and the full body of the function.

[!code-csharp[FunctionDefinition](FunctionDefinition.cs)]

## Function Declaration
[!code-csharp[Function Signatures (Prototype)](Prototype.cs)]

## Variable Declaration
IVariableDeclaration is implemented by local variable declarations and parameter declarations. The
interface abstracts the differences between the two types of variable declarations for most common
cases. Most code generation or AST consumers don't care about the differences (i.e. Parameters have
an index but locals don't)

[!code-csharp[IVariableDeclaration](IVariableDeclaration.cs)]

### Local Variable
[!code-csharp[Local Variable Declarations](LocalVariableDeclaration.cs)]

### Function Parameter
[!code-csharp[Parameter declarations](ParameterDeclaration.cs)]

## Expression
Kaleidoscope is a functional language, all expressions produce a value, even if it is always zero. There
are no statements in the language. Expressions form the core of the language and the bulk of the AST.

The IExpression interface forms the common interface for all AST expression nodes
[!code-csharp[IExpression](IExpression.cs)]

While this is an empty interface, it serves to distinguish between AST nodes that are not expressions.
Thus providing some type safety for consumers. (i.e. it makes no sense to have a prototype as the operand
for a binary operator so only nodes that implement the IExpression tag interface are allowed) This isn't
a common or generally recommended pattern for interfaces but makes sense here since some form of differentiation
is needed.

### Unary Operator
Unary operators are all user defined, so the AST simply represents them as a Function Definition. No
additional node types are needed for unary operators in the AST.

### Binary Operator
BinaryOperatorExpression covers the built-in operators, any user defined binary operators are transformed
to a function declaration/definition

[!code-csharp[BinaryOperatorExpression](BinaryOperatorExpression.cs)]

The properties are fairly self explanatory, including the kind of operator and the left and right sides of the
operator. The normal code generator pattern for the binary operators is:

1. Generate code for the left side expression to a new value
2. Generate code for the right side expression to a new value
3. Apply the operator to the generated left and right values
4. Return the result

#### Assignment
Assignment is a special kind of binary operator to represent "store" semantics for a variable. (e.g. mutable variables).
Code generation for the assignment must handle the left side operand with a slightly different pattern. In particular,
the left hand side is not an evaluated expression. Instead, it is the variable to assign the right hand value to. Thus,
there isn't anything to evaluate for the left hand side as it is always a Variable Reference for the variable to assign
the value to.

### Function Call
Calls to functions (extern, user defined operators, or user defined functions) are represented in the AST as a
FunctionCallExpression. The FunctionCallExpression contains the declaration of the function to call along with 
expressions for all of the arguments to the function.

[!code-csharp[FunctionCallExpression](FunctionCallExpression.cs)]

### Variable Reference
A variable reference is used to refer to a variable. In most cases this represents implicit "load" semantics for a
variable. However, when used as the left hand side of an assignment operator, it has "store" semantics.

[!code-csharp[VariableReferenceExpression](VariableReferenceExpression.cs)]

### Conditional
In Kaleidoscope conditional expressions follow the familiar if/then/else form, even though they are really more
like the ternary operator expression `( x ? y : z )` in C and related languages.

[!code-csharp[ConditionalExpression](ConditionalExpression.cs)]

### For-In
The for in expression is used to implement loops in Kaleidoscope.

[!code-csharp[ForInExpression](ForInExpression.cs)]

### Var-In
Var-In Expression is used to provide, potentially nested, local scopes for variables

[!code-csharp[VarInExpression](VarInExpression.cs)]

