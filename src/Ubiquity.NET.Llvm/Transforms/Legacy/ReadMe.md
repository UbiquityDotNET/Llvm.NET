## Legacy Pass Manager support
LLVM has changed the mechanisms for optimization and passes over the years. Sadly that means
there are two distinct sets of mechanics for the passes. Worse, that LLVM itself is apparently
still in transition (perhaps in permanent bifurcation?) so that BOTH forms are used depending
on the context. According to [LLVM docs](https://llvm.org/docs/WritingAnLLVMPass.html#introduction-what-is-a-pass)
the legacy pass manager is still used for code generation. Though the definition of that term
is left undefined it is assumed to mean the final stages of compilation that generate the
target machine code. This much is at least consistent with the naming of things and support in
the C-API. (You can really only add the target specific analysis passes to a legacy pass manager).
For everything else you need the new pass manager support.
