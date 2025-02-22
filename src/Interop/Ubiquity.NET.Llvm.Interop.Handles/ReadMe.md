# LLVM Handles
LLVM handles are essentially opaque pointers to a defined type. The actual
definition and bit layout of the type is not relevant to or known by the
consumer.

In LLVM handles come in three varieties
1) Global handles
2) Alias handles
3) Context handles

## Why a distinct library?
The use of a distinct library for the handles is to deal with the Roslyn
Source Generator limitation of ordering. There is no determenistic mechanism
for specifying the ordering of Source generators.
(See: [Roslyn Issue 57239](https://github.com/dotnet/roslyn/issues/57239))

By using a distinct Library, that has NO dependency ordering on the built-in
interop source genertor, it is safe to generate the body of the handles with
a source generator. The resulting code includes attributes that the built-in
interop source generator will need to generate the proper marshalling of the
handles. If these were in the same library and the built-in generator runs first,
which it normally will, then it only sees what was written directly in the C#
code. The source generator that produces the results to tell the built-in
generator how to marshal things hasn't run yet!

# Global Handles
These are top level handles that require releasing the resources they represent. The
means of accomplishing that is dependent on the type of handle. In the interop layer
these are all derived from `LlvmObjectRef`, which is derived from `SafeHandle`. Thus
all Global handles are `SafeHandle`. The specific handle type contain an override of
the `ReleaseHandle` method to clean up the resource as appropriate for the specific
type of resource it supports. Not all global handles are "owned", though any
constructed as a Global Handle are owned and require cleanup. (See [Alias Handles](#alias-handles)) 

# Alias Handles
Alias handles are Global handles that are **NOT** owned and thus do NOT release when
destroyed.

# Context Handles
Context handles are, like an alias Handle, not owned. (They are by definition never
owned). It is tempting to make these as `ref` types to let the compiler limit use to
non-heap scenarios. However, it is normally valuable to wrap them in a managed class
so the types contained here are all `readonly record struct` but NOT marked as ref.
