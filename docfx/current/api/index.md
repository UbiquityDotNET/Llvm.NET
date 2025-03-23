## Ubiquity.NET.LLvm
The Ubiquity.NET.LLvm library provides a .NET compatible OO wrapper around an extended LLVM-C
API. This allows use of the LLVM backend with a C# (or other .NET language) as the front-end.

In Version 20.1.0 a number of issues were resolved using newer .NET as well as in LLVM design
itself that allows for a fundamentally new implementation. While there isn't a LOT of code
that consumers have to change (See the samples and compare against older versions) there are
important factors to consider in the new library:
1) Ownership
    - The previous variants of the library did NOT generally consider ownership carefully. It
      routinely provided types that under some circumstances require disposal, and others did
      not (Alias). This caused problems for the internning of projected types as the behavior
      of the first instance interned was used. (Usually leading to leaks or strange crashes at
      very unrelated times).
3) No Internning of projected types
    - Projected types are no longer internned, this dramatically increases performance and
      reduces the complexity of maintenance of this library. Generally it should have little
      impact as anything that produces an alias where the type might in other cases require
      the owner to dispose it should now produce an interface.
2) Assumption of Reference Equality
    1) In the new library there is NO guarantee of reference equality for reference types.
        - Such types MAY be value equal if they refer to the same underlying native instance.
