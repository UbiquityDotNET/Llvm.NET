using System;

namespace Llvm.NET.DebugInfo
{
    // for now the DebugInfo hierarchy is mostly empty
    // classes. This is due to the "in transition" state
    // of the underlying LLVM C++ model. All of these
    // are just a wrapper around a Metadata* allocated
    // in the LLVM native libraries. The only properties
    // or methods exposed are those required by current
    // projects. This keeps the code churn to move into
    // 3.7 minimal while allowing us to achieve progress
    // on current projects.

    /// <summary>Array of see <a href="Type"/> nodes for use with see <a href="DebugInfoBuilder"/> methods</summary>
    public class DiTypeArray
    {
        internal DiTypeArray( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        internal LLVMMetadataRef MetadataHandle { get; }
    }

    /// <summary>Array of see <a href="Descriptor"/> nodes for use with see <a href="DebugInfoBuilder"/> methods</summary>
    public class DiArray
    {
        internal DiArray( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        internal LLVMMetadataRef MetadataHandle { get; }
    }

    /// <summary>Root of the object hierarchy for Debug information metadata nodes</summary>
    /// <remarks>
    /// A descriptor is just a wraper around a Metadata Node (MDNode in LLVM C++) and, unlike
    /// see <a href="Values.Value"/> they are not uniqued, although the underlying metadata nodes
    /// usually are. 
    /// </remarks>
    public class DiDescriptor
    {
        /// <summary>Dwarf tag for the descriptor</summary>
        public Tag Tag
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return (Tag)(ushort.MaxValue);

                return ( Tag )LLVMNative.DIDescriptorGetTag( MetadataHandle );
            }
        }

        public bool IsDerivedType
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsDerivedType( MetadataHandle );
            }
        }

        public bool IsCompositeType
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsCompositeType( MetadataHandle );
            }
        }

        public bool IsSubroutineType
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsSubroutineType( MetadataHandle );
            }
        }

        public bool IsBasicType
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsBasicType( MetadataHandle );
            }
        }

        public bool IsVariable
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsVariable( MetadataHandle );
            }
        }

        public bool IsSubprogram
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsSubprogram( MetadataHandle );
            }
        }

        public bool IsGlobalVariable
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsGlobalVariable( MetadataHandle );
            }
        }

        public bool IsScope
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsScope( MetadataHandle );
            }
        }
        public bool IsFile
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsFile( MetadataHandle );
            }
        }

        public bool IsCompileUnit
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsCompileUnit( MetadataHandle );
            }
        }

        public bool IsNameSpace
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsNameSpace( MetadataHandle );
            }
        }

        public bool IsLexicalBlockFile
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsLexicalBlockFile( MetadataHandle );
            }
        }

        public bool IsLexicalBlock
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsLexicalBlock( MetadataHandle );
            }
        }

        public bool IsSubrange
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsSubrange( MetadataHandle );
            }
        }
        public bool IsEnumerator
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsEnumerator( MetadataHandle );
            }
        }

        public bool IsType
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsType( MetadataHandle );
            }
        }

        public bool IsTemplateTypeParameter
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsTemplateTypeParameter( MetadataHandle );
            }
        }

        public bool IsTemplateValueParameter
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsTemplateValueParameter( MetadataHandle );
            }
        }

        public bool IsObjCProperty
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsObjCProperty( MetadataHandle );
            }
        }

        public bool IsImportedEntity
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsImportedEntity( MetadataHandle );
            }
        }

        public bool IsExpression
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return false;

                return LLVMNative.DIDescriptorIsExpression( MetadataHandle );
            }
        }

        internal DiDescriptor( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            if( MetadataHandle.Pointer == IntPtr.Zero )
                return string.Empty;

            return LLVMNative.MarshalMsg( LLVMNative.DIDescriptorAsString( MetadataHandle ) );
        }

        /// <summary>Replace all uses of this descriptor with another</summary>
        /// <param name="context">Context to use for replacement</param>
        /// <param name="other">New descriptor to replace this one with</param>
        public void ReplaceAllUsesWith( Context context, DiDescriptor other )
        {
            if( MetadataHandle.Pointer == IntPtr.Zero )
                throw new InvalidOperationException( "Cannot Replace all uses of a null descriptor" );

            LLVMNative.DiDescriptorReplaceAllUsesWith( context.ContextHandle, MetadataHandle, other.MetadataHandle );
            MetadataHandle = LLVMMetadataRef.Zero;
        }

        internal LLVMMetadataRef MetadataHandle { get; private set; }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diexpression"/></summary>
    public class DiExpression : DiDescriptor
    {
        internal DiExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }

    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diglobalvariable"/></summary>
    public class DiGlobalVariable : DiDescriptor
    {
        internal DiGlobalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsGlobalVariable( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilocalvariable"/></summary>
    public class DiLocalVariable : DiDescriptor
    {
        internal DiLocalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsVariable( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dienumerator"/></summary>
    public class DiEnumerator : DiDescriptor
    {
        internal DiEnumerator( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsExpression( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubrange"/></summary>
    public class DiSubrange : DiDescriptor
    {
        internal DiSubrange( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubrange( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>Base class for all Debug info scopes</summary>
    public class DiScope : DiDescriptor
    {
        internal DiScope( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsScope( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dicompileunit"/></summary>
    public class DiCompileUnit : DiScope
    {
        internal DiCompileUnit( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsCompileUnit( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#difile"/></summary>
    public class DiFile : DiScope
    {
        internal DiFile( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsFile( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilexicalblock"/></summary>
    public class DiLexicalBlock : DiScope
    {
        internal DiLexicalBlock( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsLexicalBlock( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilexicalblockfile"/></summary>
    public class DiLexicalBlockFile : DiScope
    {
        internal DiLexicalBlockFile( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsLexicalBlockFile( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dinamespace"/></summary>
    public class DiNamespace : DiScope
    {
        internal DiNamespace( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsNameSpace( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubprogram"/></summary>
    public class DiSubProgram : DiScope
    {
        internal DiSubProgram( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubprogram( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>Base class for Debug info types</summary>
    public class DiType : DiScope
    {
        internal DiType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

        public DebugInfoFlags Flags
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return 0;

                return ( DebugInfoFlags )LLVMNative.DITypeGetFlags( MetadataHandle );
            }
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dibasictype"/></summary>
    public class DiBasicType : DiType
    {
        internal DiBasicType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsBasicType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diderivedtype"/></summary>
    public class DiDerivedType : DiType
    {
        internal DiDerivedType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsDerivedType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dicompositetype"/></summary>
    public class DiCompositeType : DiType
    {
        internal DiCompositeType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsCompositeType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubroutinetype"/></summary>
    public class DiSubroutineType : DiCompositeType
    {
        internal DiSubroutineType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubroutineType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

    }
}
