// <copyright file="DILocalScope.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using JetBrains.Annotations;
using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Legal scope for lexical blocks, local variables, and debug info locations</summary>
    public class DILocalScope
        : DIScope
    {
        /// <summary>Gets the parent scope as a <see cref="DILocalScope"/></summary>
        [property: CanBeNull]
        public DILocalScope LocalScope => Scope as DILocalScope;

        /// <summary>Gets the DISubprogram for this scope</summary>
        /// <remarks>If this scope is a <see cref="DISubProgram"/> then it is returned, otherwise
        /// the scope is walked up to find the subprogram that ultimately owns this scope</remarks>
        public DISubProgram SubProgram
        {
            get
            {
                if( this is DILexicalBlockBase block )
                {
                    return block.LocalScope?.SubProgram;
                }

                return this as DISubProgram;
            }
        }

        /// <summary>Gets the first non-<see cref="DILexicalBlockFile"/> scope in the chain of parent scopes</summary>
        public DILocalScope FirstNonLexicalBlockFileScope
        {
            get
            {
                if( this is DILexicalBlockFile file )
                {
                    return file.FirstNonLexicalBlockFileScope;
                }

                return this;
            }
        }

        internal DILocalScope( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
