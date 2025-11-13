# About
This library contains extensions that are shared amongst multiple additional projects. This,
currently takes the place of a source generator that would inject these types. The problem
with a Roslyn source generator for this is that the "generated" sources have a dependency on
types that are poly filled by a diffent source generator. Source generators all see the same
input and therefore a source generator is untestable without solving the problem of
explicilty generating the sources for the poly filled types.
