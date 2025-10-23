# About
Ubiquity.NET.InteropHelpers helper support common to low level interop libraries. While
this library is intended to support the Ubiquity.NET.Llvm interop requirements there isn't
anything bound to that library in the support here. That is it is independent and a useful
library for any code base providing interop support.

# Key Features
* String handling
    * A lot of interop deals with strings in some form or another and handling them
      is a major amount of effort for most interop libraries. The support provided here
      enables lazy evaluation/marshalling and encoding of native strings and managed strings.
      These allow a simple `byte[]` to store a native string and ONLY marshals to a UTF16
      managed string once when needed. This allows storing and passing strings in their
      native form for FAST retrieval from a native call and then providing that same string
      as an `in` parameter in another call. All without the need to marshal from native and
      then back again just for the call. This is a MAJOR performance enhancement for APIs
      that deal in strings.
* Delegates and NativeCallbacks as Function pointers
    * Function pointers are a new feature of C# that makes for very high performance interop
      scenarios. However, sometimes the callback for a function pointer actually needs
      additional data not part of the parameters of the function to work properly. This
      library provides support for such scenarios where a delegate is used to "capture" the
      data while still supporting AOT scenarios. (NOTE: Marshal.GetFunctionPointerForDelegate()
      must dynamically emit a thunk that contains the proper signature and the captured
      "this" pointer so is NOT AOT friendly) The support offered in this library, though a
      bit more tedious, is AOT friendly.

