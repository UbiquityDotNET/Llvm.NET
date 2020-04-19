# MarshalingInfoTable
The marshaling info table provides details on the special marshaling type transforms
and attributes required to implement correct call semantics for the interop code. All
entries in the table implement the IMarshalInfo interface which, abstracts the details
of each case. There are several different implementations of the marshaling interface
for the various patterns in the LLVM-C headers.

## Array Marshaling
Array marshaling determines the in/out semantics as well as the correct element type
and size information for correct marshaling of out and return arrays.

## Out Element marshaling

