# About
Ubiquity.NET.Extensions contains general extensions for .NET. This is
a bit of a "grab bag" of functionality used by but not actually part of
multiple other Ubiquity.NET projects.

## Key support
* Computing a hash code for a ReadOnlySpan of bytes using
  [System.IO.System.IO.Hashing.XxHash3](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.xxhash3)
* DisposableAction for building actions that must occur on Dispose
    - This is useful for implementing the RAII pattern in .NET.
* MustUseReturnValueAttribute that is compatible with the [MustUseRetVal](https://github.com/mykolav/must-use-ret-val-fs)
  package.
* StringNormalizer extensions to support converting line endings of strings
  for interoperability.
* A custom ValidatedNotNullAttribute to allow compiler to assume a parameter
  value is validated as not null.
* Fluent style parameter value validation extensions.
    - These are useful when passing parameters to a function that produces a
      result that is fed to the base constructor. These are also useful in body
      expressions to validate input parameters.
* DirectoryBuilder to enable dictionary initializer style initialization with
  `ImmutableDictionary<TKey, TValue>` 
