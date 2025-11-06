# About
This folder contains fluent validation extensions. It is a distinct namespace to aid in
diasambiguation when using downlevel polyfills for static validation extensions. When both
instance extensions and static extensions are available with the same name there is an
ambiguity and the compiler resolves to the instance extension. Thus,
`<TypeName>.<ExtensionStaticMethodName>` is resolved as
`<TypeNameExtension>.<InstanceMethodName>` when both are available. Thus, when supporting
downlevel runtimes (like for a Roslyn Source generator/analyzer/fixer or VSIX extension)
then both namespaces are not implicitly "used". Instead only one is. If both namespaces are
needed in a compilation unit, then the poly fill is "used" and the fluent form is explicitly
referenced. Thus, there is no implicit ambiguity/confusion.
