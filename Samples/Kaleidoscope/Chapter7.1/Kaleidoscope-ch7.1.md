---
uid: Kaleidoscope-ch7.1
---

# 7. Kaleidoscope: Extreme Lazy JIT
In the previous chapters the code generation took an AST, converted it to LLVM IR, handed the IR to the
JIT, which then generated the native code. For a top level anonymous expression that is pretty much all
you need. But what if a function is defined but not used (yet or ever)? The process of generating the IR,
and then subsequently the native code, is all wasted overhead in such a case. That's not really following
through on the "Just-In-Time" part of the JIT. This chapter focuses on resolving that with truly lazy JIT
that doesn't even generate the LLVM IR for a function until it is called for the first time.

## Performance trade-offs
As with many things in software, there are trade-offs involved. In this case the trade-off is when you
JIT compile vs. lazy compile. This choice is a major element to efficient use of a JIT. The more you have
to JIT before anything can actually run the slower the application startup is. If you defer too much then
the execution slows down as everything needs to compile code. Ultimately, there is no one "right" solution
as many factors contribute to the results, including the level of optimizations applied during generation.
(e.g. it might achieve better results to generate unoptimized code during startup, and later regenerate
optimized versions of the most frequently used code.)

The approach to balancing the trade-offs taken in this chapter is to eagerly compile top level expressions
as it is obvious they are going to be called, and discarded afterwards. For function definitions, it isn't
clear if the functions will or won't be called. While, the code generation could scan the function to find
all functions it calls to generate them all at the same time - there is no guarantee that the input arguments
to the function will go through a path that needs them all. Thus, for Kaleidoscope, function definitions are
all lazy compiled on first use.

## General Concept of Lazy Compilation
The general idea is that the language runtime registers every lazy JIT function with the JIT by name with a
callback function to handle generating code for that function. This does two things in the JIT:
 1. Adds the name to the function symbol table in the JIT
 2. Creates a stub implementation function in native code that will call back to the JIT when application
code calls the function.

The stub is implemented by the JIT to call back into the JIT in a way that includes the information needed
to identify the correct function to generate code for. The JIT will do some of it's own internal setup and
then call the code generation callback registered by the runtime code generator. This callback is what actually
generates the LLVM IR, and ultimately the native code, for the function.

Once the function is generated the generator uses the JIT to update the stub so that, in the future, it will just
call to the generated function directly. One somewhat confusing aspect of this is that there are two symbols in
the JIT for what is really only one function. One, is the stub that remains at a fixed location (to allow pointer
to function patterns to work) the other is the JIT compiled actual implementation of the function. They can't both
have the same name so the code generation for the implementation must use a unique name.

## Code changes for lazy JIT
### Initialization
The LLVM ORC JIT v2 uses a mult-layered system for materializing the IR and eventually the native executbable
code. The Kaleidoscope JIT includes transfomrs of IR modules to support setting the data layout for the module
to match the JIT and also to run optimization passes on the module. To support lazy evaluation a few such 
components are needed for the code generator. These are setup in the constructor and destroyed in the Dispose
method.

[!code-csharp[PrivateMembers](CodeGenerator.cs#PrivateMembers)]

[!code-csharp[Initialization](CodeGenerator.cs#Initialization)]

[!code-csharp[Dispose](CodeGenerator.cs#Dispose)]


### Body implementation
Since the lazy JIT registers the callback stub with the function's name when the actual function is generated
it needs a new name for the backing body. So, we add a new helper method to effectively clone a FunctionDefinition
AST node while renaming it. This only needs a shallow clone that changes the name so there isn't a lot of
overhead for it. (Theoretically, this could be done with a readonly struct and 'with', such an optimization
is left as an excercise for the reader :nerd_face:)

[!code-csharp[CloneAndRenameFunction](CodeGenerator.cs#CloneAndRenameFunction)]

The name used for the body is the original function name plus the suffix `$impl` tacked onto the end. This
suffix was chosen as it includes characters not allowed within the Kaleidoscope language so there is no
possibility of a name collision.

### Code generation
The next requirement is to change how we generate the functions. For an anonymous function the generation
is pretty much the same. There's really no point in going through the process of setting up the lazy JIT
when the next thing to do is get the address of the function and call it. For other definitions, though,
things get different as they are selected for lazy JIT.

[!code-csharp[Generate](CodeGenerator.cs#Generate)]

Function definitions for lazy JIT are first cloned and renamed, as discussed previously. Then a lazy
function generator is registered for the name of the function. This creates the stub function exported
by the function's name with a callback that knows how to generate the LLVM IR for the function. The
actual code generation call back is a lambda that simply initializes a new module and pass manager,
generates the function using the visitor pattern and returns the function's implementation name and
the containing module as a tuple. (This is where keeping the code generation ignorant of the JIT comes
in handy as the same code is called to generate a module and doesn't need to care if it is eager or lazy)

The JIT implementation will do the following after the generator
callback returns:
 1. Add the returned module to the JIT
 2. Generate native code for the module
 3. Get the address of the implementation function
 4. Update the stub for the function with the address of the function instead of the internal callback
 5. return the address to the JIT engine so it can ultimately call the function and continue on it's merry way.

#### Lazy Materializer
The bulk of the work is in the ORCJIT v2 implementation however kaleidoscope must "hook" into the support
there to provide a materializer that can convert the AST into an LLVM IR. Technically it provides an LLVM
module for a symbol (the body implementation name). The JIT couldn't care less about the AST. The materializer
will generate the IR for a given symbol by processing the AST into a module and providing that to the JIT.

[!code-csharp[AddLazyMaterializer](CodeGenerator.cs#AddLazyMaterializer)]

## Conclusion
Implementing Lazy JIT support with Ubiquity.NET.Llvm is a bit more complex, but still not significant. It
took many times more words to describe then actual lines of code. Efficiently, supporting lazy JIT is a
much more complex matter. There are trade-offs doing things lazy, in particular the application can stall
for a period, while the system generates new code to run "on the fly". Optimizations, when fully enabled,
add additional time to the code generation. While, for some applications, it may be obvious whether these
factors matter or not, in general it's not something that can be known, thus the quest for optimal
efficiency includes decisions on eager vs lazy JIT as well as optimized JIT or not. This can include lazy
JIT with minimal optimization during startup of an app. Once things are up and going the engine can come
back to re-generate the functions with full optimization. All sorts of possibilities exist, but the
basics of how the lazy and eager generation works doesn't change no matter what approach a given language
or runtime wants to use.
