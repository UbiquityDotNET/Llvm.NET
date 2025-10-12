# Ubiquity.NET.Llvm.Interop
This library contains the low level interop between managed code and the native library.
(Ubiquity.NET.LibLLVM). 

>[!IMPORTANT]
> Consumers should ***NOT*** use this directly, instead they should use the [`Ubiquity.NET.Llvm`](https://www.nuget.org/packages/Ubiquity.NET.Llvm)
> package. That package includes a tested OO wrapper around the low level interop API. This
> package may not exist in the future as the functionality might fold into the OO wrapper
> library and any app built based on it would be stuck (on your own)  
>
> :warning: You have been warned! :warning:

## Why not Direct P/Invoke?
[This is a thing: see some of the limited [docs](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/interop)
for more info]  

Firstly it's young, poorly understood, barely even documented and not what is considered
by many as stable/mature yet. But, mostly because it requires everything to compile AOT to work. That
is, it's all or nothing and doesn't support use in an either or model (traditional JIT runtime,
or AOT). Somethings are not yet supporting AOT/Trimming scenarios. (In the case of the samples
in this repo the DGML graph builder used employs XML serialization, which in turn requires
dynamic reflection to work. This required a custom solution to resolve. So, as great a feature
AOT is it is not without issues at present. Perhaps over time it will become the normal state
and more libraries will build in a way as to support it. (This library has a motivation to go
there in that it's broader use is specifically for AOT code generation! Thus it's a bit on the
"bleeding edge" of things.)
