// <copyright file="ModuleFlag.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

namespace Llvm.NET
{
    /// <summary>Module Flags Tuple for a module</summary>
    public class ModuleFlag
    {
        /// <summary>Initializes a new instance of the <see cref="ModuleFlag"/> class.</summary>
        /// <param name="behavior">Behavior for the flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="metadata">Metadata for the flag</param>
        public ModuleFlag( ModuleFlagBehavior behavior, string name, LlvmMetadata metadata )
        {
            Behavior = behavior;
            Name = name;
            Metadata = metadata;
        }

        /// <summary>Gets the <see cref="ModuleFlagBehavior"/> options for this module flag</summary>
        public ModuleFlagBehavior Behavior { get; }

        /// <summary>Gets the name of flag</summary>
        public string Name { get; }

        /// <summary>Gets the Metadata for this flag</summary>
        public LlvmMetadata Metadata { get; }

        internal ModuleFlag( MDNode node )
        {
            node.ValidateNotNull( nameof( node ) );
            if( node.Operands.Count != 3 )
            {
                throw new ArgumentException( "Expected node with 3 operands", nameof( node ) );
            }

            if( !(node.Operands[0] is ConstantAsMetadata behavior))
            {
                throw new ArgumentException( "Expected ConstantAsMetadata for first operand", nameof( node ) );
            }

            if( !(behavior.Constant is ConstantInt behaviorConst ) )
            {
                throw new ArgumentException( "Expected ConstantInt wrapped in first operand", nameof( node ) );
            }

            if( !(node.Operands[1] is MDString nameMd ) )
            {
                throw new ArgumentException( "Expected MDString as second operand", nameof( node ) );
            }

            Behavior = ( ModuleFlagBehavior )( behaviorConst.ZeroExtendedValue );
            Name = nameMd.ToString( );
            Metadata = node.Operands[ 2 ];
        }
    }
}
