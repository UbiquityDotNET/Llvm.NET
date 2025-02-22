# LlvmHandleGenerator
This source generator is used to create the code for the many types of handles
from LibLLVM. These support safe marshalling/cleanup with clear ownership semantics
using `GlobalHandleBase` for global handles and `IContextHandle<T>` for contextual
handles that are always a reference to something owned by the container.

This generator is tightly bound to `Ubiquity.NET.Llvm.Interop.Handles` library and
assumes attributes and types within it. 
