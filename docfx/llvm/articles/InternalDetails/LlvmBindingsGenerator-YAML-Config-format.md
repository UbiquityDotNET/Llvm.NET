# YAML Configuration file format
>[!WARNING]
> The API marshalling documented here is obsoleted in V20+. This document needs cleanup to clarify newer
> more limited scope/functionality.

## FunctionBindings
The functionBindings contains all the information for properly marshaling the native C APIs especially disambiguation of
overly generic data types like `char*` (Is that a pointer to a single character, an array of characters, a zero terminated string,
or perhaps a pointer to an unsigned byte value... [Inquiring minds want to know ;)]). This section is responsible for providing
the details for any data types that don't have an implicit mapping into .NET. The properties for the entry are:

| Name        | Type   | Description |
|-------------|--------|-------------|
| Name        | string | Exported "C" name of the function this binding applies to |
| IsObsolete  | bool   | Indicates if the function is considered obsolete [default is false] |
| DeprecationMessage | string | Deprecation message to apply to the projection (Required if IsObsolete is true) |
| IsExported  | bool   | Indicates if the function is exported from LibLLVM.dll [Default is true] |
| IsProjected | bool   | Indicates if the function is projected to .NET via Ubiquity.NET.Llvm.Interop.dll [Default is true] |
| ReturnTransform | MarshalingInfo | Optional marshaling info for the return value |
| ParamTransforms | Collection of MarshalingInfo | Optional collection of marshaling info for any parameters that need special handling |

Example:
```yaml
FunctionBindings:
# LLVMConstGEP2 appears in Core.h, but has no implementation [Go, Figure!]
  - Name: LLVMConstGEP2
    IsExported: False
    IsProjected: False

  - Name: LLVMTargetMachineEmitToFile
    ReturnTransform: !Status {}
    ParamTransforms:
      - !String {Name: ErrorMessage, Semantics: Out, Kind: DisposeMessage }
      - !String {Name: Filename, Semantics: In, Kind: CopyAlias }

  - Name: LLVMDIBuilderCreateExpression
    ParamTransforms:
      - !Array {SubType: I8, Name: Addr, Semantics: In }

```
### MarshalingInfo
The marshaling info in the FunctionBindings table provides details on the special marshaling
type transforms and attributes required to implement correct call semantics for the interop code.
All entries in the table implement the IMarshalInfo interface which, abstracts the details of
each case. There are several different implementations of the marshaling interface for the
various patterns in the LLVM-C headers.

#### Base properties for marshaling
There are a number of different types of marshaling with a common set of properties for all. Thus a base type is defined for them
and each individual marshaling type includes these common properties.

| Property name | Description |
|---------------|-------------|
| Name          | Name of the parameter (ignored for return types) |
| IsAlias       | Indicates if the return is an alias handle (Only applies to a return) |
| IsUnsafe      | Indicates if the return type in managed code is unsafe (Only applies to return)|
| Semantics     | Declares the semantics of the parameter (Return, In, Out, InOut - see table below for details) |

##### Semantics
The semantics property defines the usage pattern for the parameter and may be one of the following values:

| Value  | Description |
|--------|-------------|
| Return | The binding applies to the return of the function (Implicitly applied for all Return Transforms and invalid on any ParamTransforms) |
| In     | The binding is for an input parameter |
| Out    | The binding is for an output parameter |
| InOut  | The binding is for an in/out (e.g., 'ref') parameter |


#### Marshaling Info derived types
There are several types of derived marshaling info for specific uses these types are detailed here.

##### Status
This is used to transform the native return type to a projected LLVMStatus to ensure it isn't confused
with a generic bool or integral value. Normally this is applied to some functions returning LLVMBool,
where the semantics of the returned value is 0 == success. There are a few APIs in the LLVM-C surface
where the native API returns a simple integral value. Using this in the return forces treatment as a
status value instead of a bool with success == true.

> This marshaling info is only valid if Semantics == Return

##### Alias
Indicates the return of a function is an alias pointer/handle type that the caller cannot release
and should not store.

##### Unsafe
Indicates the return is an unsafe pointer type (This basically disables the validation pass check
for unsafe types for this function)

##### String
Indicates the parameter or return is a string along with specifics about the kind of string which
implies the method of releasing the string data (if any). This type has the additional Kind property:

###### String.Kind
Tye kind property declares the type of marshaling to apply for the string. In all cases the string is
copied to a managed string during marshaling. [A future optimization might be able to provide a means
to use a Span of UTF8 chars for in-place zero-copy strings but that is not yet implemented] The following
table provides details of the different kinds of string marshaling supported:

| Name      | Description |
|-----------|-------------|
| CopyAlias | Copy only, no release of the native string |
| DisposeMessage | Calls LLVMDisposeMessage() to release the native string |
| OrcDisposeMangledSymbol | Calls LLVMOrcDisposeMangledSymbol() to release the native string |
| DisposeErrorMessage | Calls LLVMDisposeErrorMessage() to release the native string |

##### Primitive
Indicates the value is marshaled as a CppSharp.AST.PrimitiveType these all have internal mappings to
equivalent .NET types.

##### Array
This indicates marshaling of an array of data, including a size. This type of marshaling info contains
the following additional properties:

| Name | Description |
|------|-------------|
| SubType | The unmanaged type of the elements of the array (See: System.Runtime.InteropServices.UnmanagedType enum for details) |
| SizeParam | Integer index of an out parameter that contains the size of the array (required for out parameters, optional for in parameters |

## HandleMap
The HandleMap table provides the link between various "handle" types in the native API and the means for properly
closing/disposing of them. This information is used to generate the appropriate .NET SafeHandle types used at the interop layer.
The elements in the map are either a `GlobalHandle` or a `ContextHandle`.

> A key aspect of handles in the OO projection is the
automatic interning of the projected type such that there is a 1:1 correlation between a handle and an instance of a wrapper for
that handle. This ensures that reference equality of the projected types "just works" no matter how the instances are acquired.

Example:
```yaml
HandleMap:
  - !GlobalHandle {HandleName: LLVMModuleRef, Disposer: LLVMDisposeModule, Alias: True}
  - !ContextHandle {HandleName: LLVMTypeRef }
# ...
```

### GlobalHandle
A global handle is one that is not implicitly owned by a container type. In LLVM most, but not all, handles refer to an object owned
by some other container. A Global handle is used for types that don't conform to that model and are instead basically free standing
allocations. These require dispose/close cleanup code when no longer needed. Thus, a global handle will have support for automatic
release of the resource. Additionally, sometimes a type normally represented via a global handle may appear as an out parameter or
return value of a function but that value is only an alias of an internally owned handle (like a reference in C++). In such cases a
special handle type with the suffix `Alias` is used to clearly indicate the intent. The alias handle types do NOT perform any cleanup
as they don't actually own the data. The properties of a GlobalHandle entry in the HandleMap table are:

| Name | Description |
|------|-------------|
| HandleName | Name of the handle type (in "C")) |
| Disposer   | Name of the C function that disposes this handle type |
| Alias      | Boolean flag to indicate if this handle requires an alias type for use as a return value or out parameter |

### ContextHandle
Context Handles are assumed owned by some context (Often, but not always, the literal `Context` type). These are simpler types
as they are always aliases and never perform any cleanup. The only property for a ContextHandle is the `HandleName`, which provides
the name of the type in `C` to allow generation of a properly named projected safe handle.

## AnonymousEnums
C/C++ allow anonymous enums that effectively introduce a const into the parent namespace.
Generally, that's not desired for the interop. In fact the only known use cases for this
in the LLVM-C headers is to handle some FLAGS type enums where the language limits the values
of an enum to the range of an int. So what LLVM does is define the anonymous enum, and then
define a typedef of unsigned to a name for the enum. (see: Core.h\LLVMAttributeIndex as an
example)

This table maps the name of the first element of an anonymous enum with the name of the
typedef used to represent the enumeration. This is used to find the correct declaration
in the AST

Example:
```Yaml
AnonymousEnums:
    LLVMAttributeReturnIndex: LLVMAttributeIndex
    LLVMMDStringMetadataKind: LLVMMetadataKind
```
## IgnoredHeaders
This section lists any headers in the input folders that do not participate in code generation and should be ignored.

Example:
```yaml
IgnoredHeaders:
  - llvm-c/LinkTimeOptimizer.h
  - llvm-c/lto.h
  - llvm-c/Remarks.h
```

