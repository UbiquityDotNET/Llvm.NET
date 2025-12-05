## ReferenceEqualityAnalyzer
This analyzer was designed to identify places within the Ubiquity.NET.Llvm library where the
transition away from interning impacts the code base, as well as aid consumers in detecting
problems with ambiguity. The transition means any code doing a reference equality check,
assuming that all unique instances are interned would get a big surprise as they are no
longer guaranteed unique and will fail for things that are really the same. (To handle the
problem of unowned "aliasing", which existed in the previous releases as well!). Thus, this
analyzer will flag as an error any use of operator `==` or `!=` that results in reference
equality where one of the types involved implements value equality (via `IEquatable` or
`IEquatable<T>`. This is almost ALWAYS an error in the consuming code. In any case where it
isn't the code should be converted to use an explicit call to `object.ReferenceEquals()` so
that it is explicitly clear what the intent is.

## IDs
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
UNL000 | Internal | Error | Diagnostics
UNL001 | Usage | Error | Diagnostics


### UNL000
An internal error (exception) was detected in the analyzer itself. This is unlikely to occur
but any occurrences of this are an error in the analyzer itself that should be [reported](https://github.com/UbiquityDotNET/Llvm.NET/issues/new/choose)
(and fixed) as a bug.

### UNL001
Reference equality was inferred but value equality is available. This is an indication of
some part of the code base that has not properly transitioned or new code that is
implemented incorrectly. If, and ONLY if, reference equality is actually intended then the
call-site should use `object.ReferenceEquals()` and add an explanatory comment.

