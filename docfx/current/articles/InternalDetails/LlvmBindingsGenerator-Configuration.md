# LlvmBindings Configuration
The bindings generator uses a configuration data structure that consists of a
number of tables used by the code generation passes for resolving the various
ambiguities of generating the code. The configuration is an instance of the
`LlvmBindingsGenerator.Configuration.GeneratorConfig` class.

The GeneratorConfig class holds all the data used by the various passes in
LlvmBindingsGenerator to transform the AST so that subsequent stages will 
generate correct .NET interop code. There are several tables of information
used in the various passes. (See: [YAML Configuration format](LlvmBindingsGenerator-YAML-Config-format.md)
For details of the configuration file YAML format)

## StatusReturningFunctions
This table is a simple [SortedSet](xref:System.Collections.Generic.SortedSet`1) of
strings. Each entry is the name of an LLVM function that semantically returns a status.
The return type is transformed to an LLVMStatus to differentiate such types from a
boolean or other integral values (in the case of a C Declaration that simply returns an
int as a status value) This helps to keep the interop layer clean, and semantically self
consistent, without actually breaking the ABI of the LLVM-C APIs. 

## MarshalingInfo
This is a collection of `LlvmBindingsGenerator.IMarshalInfo` instances
used to transform function parameter and return types as well as add appropriate marshaling
attributes for them. This is the most complex implementations of all the passes. Though
declaring the table is made a lot easier by the various IMarshalInfo implementation types.
(see: [YAML Configuration format](LlvmBindingsGenerator-YAML-Config-format.md) for details)

## DeprecatedFunctionToMessageMap
This property consists of a dictionary that maps the name of a deprecated LLVM-C API to
a message explaining what the alternative is, if there is one. This is used to create the
declaration with the Obsolete attribute in .NET. 
>[!NOTE]
>At present this table is only used to completely filter out the deprecated functions so they
>do not even appear in the generated output or EXPORTS.

## InternalFunctions
This property contains a dictionary of functions that are considered internal or just completely
ignored for the interop layer. Generally these are the disposal methods that are used in the
generated handle types or custom string marshalers. Sometimes they are for functions declared in
the headers but not implemented in the library. (the LTO APIs for instance are not included as they
are intended for use in an actual linker) The values in the dictionary are a boolean that indicates
if the function is just completely ignored (true) or used internally only (false). The
MarkFunctionsInternalPass uses this information to mark the functions appropriately for subsequent
passes.

## HandleToTemplateMap
The LlvmBindings generator supports creating safe handles for all of the LLVM object handle types.
Since there are a few distinct patterns for handles the HandleToTemplate map is used to map between
the LLVM-C typedef handle name and the actual template that will generate the code for that
particular handle. The HandleToTemplateMap property is a keyed collection of IHandleCodeTemplate
that is keyed by the HandleName. There are, at present two general template types:

### GlobalHandleTemplate
This is used for handles that have a global application defined lifetime and therefore have a
distinct disposal method. The generated handle types derive from the .NET SafeHandle types for
marshaling. The following is a summary of the handle types (for more details see
[Handle Marshaling](llvm-handles.md))

#### Alias handles
On occasion there are places where the LLVM-C APIs retrieve a handle to an object that is just
an alias (e.g. not ultimately owned by the caller after the call completes). These are usually,
but not always, child objects returning a reference to the parent. In such cases the application
should never dispose the handle as it doesn't own it.

### ContextHandleTemplate
Contextual handles are those that are always references to objects owned by a parent container
of some sort. Most objects in LLVM are owned by an LLVM Context or module and only disposed of
when the parent container itself is disposed.

## AnonymousEnumNames
The AnonymousEnumNames is a dictionary that maps the name of the first enumerated value of an
anonymous enum with the typedef type that is used to represent it. This table is used to
effectively de-anonymize the enum so that a .NET enum with the correct underlying type is
generated.

## IgnoredHeaders
This contains a list of header files that the IgnoreSystemHeadersPass uses to mark some of the
LLVM headers as ignored. Such headers will not produce any generated output for .NET.

## AliasReturningFunctions
This property contains a list of function names that return a handle, that is, in reality, an
alias that the caller **MUST NOT** destroy/dispose.
