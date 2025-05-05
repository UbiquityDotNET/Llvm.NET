# ReferenceQualityAnalyzer
This analyzer was designed to identify places within the Ubiquity.NET.Llvm library where the transition
away from interning impacts the code base. The transition means any code doing a reference equality
check, assuming that all unique instances are interned would get a big surprise as they are no longer
guaranteed unique. (To handle the problem of unowned "aliasing", which existed in the previous releases
as well!). Thus, this analyzer will flag as an error any use of operator == or != that results in
reference equality where one of the types involved implements value equality. This is almost ALWAYS an
error in the consuming code. In any case where it isn't the code should be converted to use an explicit
call to ReferenceEquals() so that it is clear what the intent is.
