// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Array of <see cref="DILocalVariable"/> nodes for use with see <see cref="DIBuilder"/> methods</summary>
    [SuppressMessage( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Name matches underlying LLVM and is descriptive of what it is" )]
    public class DILocalVariableArray
        : TupleTypedArrayWrapper<DILocalVariable>
    {
        internal DILocalVariableArray( MDTuple tuple )
            : base( tuple )
        {
        }
    }
}
