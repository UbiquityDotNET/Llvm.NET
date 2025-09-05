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
  for interoperability across OS platforms and compatibility with "on disk" representations.
* A custom ValidatedNotNullAttribute to allow compiler to assume a parameter
  value is validated as not null.
* Fluent style parameter value validation extensions.
    - These are useful when passing parameters to a function that produces a
      result that is fed to the base constructor. These are also useful in body
      expressions to validate input parameters.
* DictionaryBuilder to enable dictionary initializer style initialization of
  `ImmutableDictionary<TKey, TValue>` with significantly reduced overhead.
    - This leverages an `ImmutableDictionary<TKey, TValue>.Builder` under the hood to build the
      dictionary. When the `ToImmutable()` method is called the builder is converted to the
      immutable state without any overhead of a copy or re-construction of hash tables etc...
* KvpArrayBuilder to enable array initializer style initialization of
  `ImmutableArray<KeyValuePair<TKey, TValue>>` with significantly reduced overhead.
    - This leverages an `ImmutableArray<T>.Builder` under the hood to build the array directly.
      When the `ToImmutable()` method is called the builder is converted to the immutable state without any
      overhead of a copy.
    - Since this is an array and not a dictionary there is no overhead for allocating, initializing or copying
      any hash mapping for the keys.

## Fluent Validation
The library includes extensions that support fluent validation to allow use in property
accessors and constructors that forward values to a base or another constructor. This is
normally used when the value itself isn't passed on but some transformed value is.

* `ThrowIfNull()`
* `ThrowIfOutOfRange()`
* `ThrowIfNotDefined()`

## PolyFill
While it isn't `impossible` to make polyfill this to work on .NET standard 2.0 it is a HUGE
amount of effort to do so and there's no compelling reason to do so. Roslyn extensions/VSIX
extensions are the most likely candidates left. VS extensions are shifting to external
processes for extensions for this reason, leaving Roslyn generators. The bottom line is
that too much of MODERN .NET and C# is incompatible with .NET Standard 2.0 that a distinct
PolyFill library can only cover some of it. (Even the PolySharp generator covers only
some of the missing functionality)
