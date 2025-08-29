// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Array of <see cref="DICompositeType"/> debug information nodes for use with <see cref="DIBuilder"/> methods</summary>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This matches the wrapped native type" )]
    public class DICompositeTypeArray
        : TupleTypedArrayWrapper<DINode>
    {
        internal DICompositeTypeArray( MDTuple? tuple )
            : base( tuple )
        {
        }
    }
}
