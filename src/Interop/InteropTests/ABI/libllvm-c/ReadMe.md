# LibLLVM Interop Tests
These tests focus on validating the extended C API unique to LibLLVM. They do NOT
test the official LLVM-C API nor it's interop bindings. (That would be excessive and
likely redundant as LLVM itself tests the APIs better, and the callers are tested in
the wrapper libraries in this repo.) The LibLLVM methods are unique to this library
and need testing at this low level to ensure that the upper layers can rely on them
to behave as expected. There is no native code testing of these APIs (at least not at
this point) so this is the only "line of defense" for them.

>[!IMPORTANT]
> As of this writing most are completely empty tests that don't do anything. These
> will come online in subsequent builds. In many cases the value of tests at this
> low level is debatable. Is it worth 300 lines of complex setup to test one function
> call? Especially when the upper layers do exercise the functionality?
