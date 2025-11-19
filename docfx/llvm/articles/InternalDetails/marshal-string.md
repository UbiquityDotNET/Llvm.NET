# Marshaling strings in Ubiquity.NET.Llvm
LLVM provides strings in several forms and this leads to complexities for P/Invoke
signatures as sometimes the strings require some form of release and in other cases, they do
not. Standard .NET marshaling of strings makes some assumptions with regard to strings as a
return type that make the LLVM APIs difficult. (e.g. in some LLVM APIs the returned string
must be released via LLVMDisposeMessage() or some other call, while in other cases it is
just a pointer to an internal const string that does not need any release.)

To resolve these issues and make the requirements explicitly clear and consistent
`Ubiquity.NET.Llvm.Interop` uses custom marshaling of the strings to mark the exact behavior
directly on the P/Invoke signature so it is both clear and easy to use for the upper layers
(In most cases this is a `LazyEncodedString` but for a few it's just a `System.String`)

## LazyEncodedString
The `Ubiquity.NET.Llvm.Interop` library makes extensive user if the
[LazyEncodedString](https://ubiquitydotnet.github.io/Ubiquity.NET.Utils/interop-helpers/api/Ubiquity.NET.InteropHelpers.LazyEncodedString.html)
type. This allows for minimal overhead marshaling of strings to and from native code. In,
particular it lazily encodes into either form depending on what it started with. It does
this exactly once. So that the overhead of encode/decode is realized only the first time it
is needed.
