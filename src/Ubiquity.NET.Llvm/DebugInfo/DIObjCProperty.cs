// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Objective-C Property</summary>
    public class DIObjCProperty
        : DINode
    {
        /*
        public uint Line {get;}
        public uint Attributes {get;}
        */

        /// <summary>Gets the Debug information for the file containing this property</summary>
        public DIFile File => GetOperand<DIFile>( 1 )!;

        /// <summary>Gets the name of the property</summary>
        public string Name => GetOperandString( 0 );

        /// <summary>Gets the name of the getter method for the property</summary>
        public string GetterName => GetOperandString( 2 );

        /// <summary>Gets the name of the setter method for the property</summary>
        public string SetterName => GetOperandString( 3 );

        /// <summary>Gets the type of the property</summary>
        public DIType Type => GetOperand<DIType>( 4 )!;

        internal DIObjCProperty( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
