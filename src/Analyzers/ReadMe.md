# Repository Analyzers
This folder contains the analyzers that are specific to this repository. They are not
generalized to a package as they are very specific to the code in this repo.

## Extension Keyword Analyzer
This repository does NOT use the new C# 14 extension syntax due to several reasons.
1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006
    1. Sadly marked as "not planned" - e.g., dead-end
    1. [New issue created](https://developercommunity.visualstudio.com/t/VS2026-Codelens-does-not-appearwork-f/10988233)
        1. Still awaiting response/results...
2) MANY analyzers get things wrong and need to be suppressed
    1. (CA1000, CA1034, and many others [SAxxxx])
3) Many tools (like docfx) don't support the new syntax yet.

Bottom line it's a good idea with an incomplete implementation lacking support in the
overall ecosystem. Don't use it unless you absolutely have to until all of that is sorted
out.

# Reference Equality Analyzer
Reference equality is usually the wrong behavior for comparing wrapped LLVM types. This, is
a significant breaking change from older releases of this library. However, due to issues
with caching (and more importantly, resolving) the correct thing (disposable or just an
alias?) - the behavior had to change and reference equality is broken. This analyzer reports
issues if the code contains a reference equality (operator == on a ref type) when the type
implements `IEquatable<T>`. This eliminates source use assumptions in this library making
the transition easier and preventing new cases of problems.
