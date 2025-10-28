# Rpository Analyzers
This contains the analyzers used that are specific to this repository.

## IDs
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
UNL000 | Internal | Error | Diagnostics, [Documentation](#UNL000)
UNL001 | Usage | Error | Diagnostics, [Documentation](#UNL001)
UNL002 | Usage | Error | Diagnostics, [Documentation](#UNL002)


### UNL000
An internal error (exception) was dedected in the analyzer itself. This is unlikely to occur
but any occurances of this are an error in the analyzer itself that should be reported (and
fixed) as a bug.

### UNL001
Reference equality was inferred but value equality is available. This is an indication of
some part of the code base that has not properly transisitioned or new code that is
implemented incorrectly. If, and ONLY if, reference equality is actually intended then the
callsite should use `ReferenceEquality()` and add an explanitory comment.  
See [ReferenceEqualityAnalyzer](#ReferenceEqualityAnalyzer)

### UNL002
C# 14 `extension` keyword used. This is currently blocked in this repository. Hopefully,
this is short term but is not allowed. This prevents any current/experimental use and any
future use. The experiments with using it have unveiled a lot of issues that make that an
incomplete feature. "Complete" requires proper support in the majority of analyzers and
third party tools. That support does not exist yet.

## ReferenceEqualityAnalyzer
This analyzer was designed to identify places within the Ubiquity.NET.Llvm library where the
transition away from interning impacts the code base. The transition means any code doing
a reference equality check, assuming that all unique instances are interned would get a big
surprise as they are no longer guaranteed unique. (To handle the problem of unowned
"aliasing", which existed in the previous releases as well!). Thus, this analyzer will flag
as an error any use of operator == or != that results in reference equality where one of the
types involved implements value equality. This is almost ALWAYS an error in the consuming
code. In any case where it isn't the code should be converted to use an explicit call to
ReferenceEquals() so that it is clear what the intent is.

## Extension Keyword used
This analyzer detects and reports as an error any use of the C# 14 "extension" keyword. It
is a great idea, but not yet implemented completely. While the language compiler itself is
complete the tools that ship with it are not. Even the first party analyzers don't support
it correctly. Additionally, third party analyzers and tools like DOCFX don't recongnize it
and either crash or handle it incorrectly.
