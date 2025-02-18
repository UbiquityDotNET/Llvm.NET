# LLVM Handles
LLVM handles are essentially opaque pointers to a defined type. The actual
definition and bit layout of the type is not relevant to or known by the
consumer.

In LLVM handles come in three varieties
1) Global handles
2) Alias handles
3) Context handles

## Global Handles
These are top level handles that require releasing the resources they represent. The
means of accomplishing that is dependent on the type of handle. In the interop layer
these are all derived from `LlvmObjectRef`, which is derived from `SafeHandle`. Thus
all Global handles are `SafeHandle`. The specific handle type contain an override of
the `ReleaseHandle` method to clean up the resource as appropriate for the specific
type of resource it supports. Not all global handles are "owned", though any
constructed as a Global Handle are owned and require cleanup. (See [Alias Handles](#alias-handles)) 

## Alias Handles
Alias handles are Global handles that are **NOT** owned and thus do NOT release when
destroyed.

## Context Handles
Context handles are, like an alias Handle, not owned. (They are by definition never
owned). It is tempting to make these as `ref` types to let the compiler limit use to
non-heap scenarios. However, it is normally valuable to wrap them in a managed class
so the types contained here are all `readonly record struct` but NOT marked as ref.

### Cannot use Roslyn source generator
It is also tempting (and an early attempt was made) to use a Roslyn source generator
for each of these. However, since those have no way to specify ordering and the
`LibraryImportAttribute` is used in the library to support AOT. There is an inherant
conflict with the `LibraryImportAttribute` requiring marshalling for the handle, but
it isn't fully declared so the `NativeMarshalling` attribute doesn't exist for the
source generator to see. Thus these were all generated from the `LlvmBindingsGenerator`
from a basic template to create each distinct type with the proper marshalling attribute.