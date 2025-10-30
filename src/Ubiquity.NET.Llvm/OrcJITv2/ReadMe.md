# LLVM ORC JIT v2 support

## Known issues
There is a [known bug](https://github.com/llvm/llvm-project/issues/65641) with LLVM JIT
object creation. The default LLJIT exposed by the LLVM-C API library does NOT use JitLink
as recommended. Changing to support that, may alleviate the problem. But, as of this build
it can still happen. Experience has shown it only appears to hit for a Release build with
an attached debugger. So, it's a very small window of opportunity to hit but when it does,
it's a HARD crash.
