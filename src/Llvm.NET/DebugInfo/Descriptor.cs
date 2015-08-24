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
    public class TypeArray
    {
        internal TypeArray( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        internal LLVMMetadataRef MetadataHandle { get; }
    }

    /// <summary>Array of see <a href="Descriptor"/> nodes for use with see <a href="DebugInfoBuilder"/> methods</summary>
    public class Array
    {
        internal Array( LLVMMetadataRef handle )
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
    public class Descriptor
    {
        /// <summary>Dwarf tag for the descriptor</summary>
        public Tag Tag => (Tag)LLVMNative.DIDescriptorGetTag( MetadataHandle );

        public bool IsDerivedType => LLVMNative.DIDescriptorIsDerivedType( MetadataHandle );
        public bool IsCompositeType => LLVMNative.DIDescriptorIsCompositeType( MetadataHandle );
        public bool IsSubroutineType => LLVMNative.DIDescriptorIsSubroutineType( MetadataHandle );
        public bool IsBasicType => LLVMNative.DIDescriptorIsBasicType( MetadataHandle );
        public bool IsVariable => LLVMNative.DIDescriptorIsVariable( MetadataHandle );
        public bool IsSubprogram => LLVMNative.DIDescriptorIsSubprogram( MetadataHandle );
        public bool IsGlobalVariable => LLVMNative.DIDescriptorIsGlobalVariable( MetadataHandle );
        public bool IsScope => LLVMNative.DIDescriptorIsScope( MetadataHandle );
        public bool IsFile => LLVMNative.DIDescriptorIsFile( MetadataHandle );
        public bool IsCompileUnit => LLVMNative.DIDescriptorIsCompileUnit( MetadataHandle );
        public bool IsNameSpace => LLVMNative.DIDescriptorIsNameSpace( MetadataHandle );
        public bool IsLexicalBlockFile => LLVMNative.DIDescriptorIsLexicalBlockFile( MetadataHandle );
        public bool IsLexicalBlock => LLVMNative.DIDescriptorIsLexicalBlock( MetadataHandle );
        public bool IsSubrange => LLVMNative.DIDescriptorIsSubrange( MetadataHandle );
        public bool IsEnumerator => LLVMNative.DIDescriptorIsEnumerator( MetadataHandle );
        public bool IsType => LLVMNative.DIDescriptorIsType( MetadataHandle );
        public bool IsTemplateTypeParameter => LLVMNative.DIDescriptorIsTemplateTypeParameter( MetadataHandle );
        public bool IsTemplateValueParameter => LLVMNative.DIDescriptorIsTemplateValueParameter( MetadataHandle );
        public bool IsObjCProperty => LLVMNative.DIDescriptorIsObjCProperty( MetadataHandle );
        public bool IsImportedEntity => LLVMNative.DIDescriptorIsImportedEntity( MetadataHandle );
        public bool IsExpression => LLVMNative.DIDescriptorIsExpression( MetadataHandle );

        internal Descriptor( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        /// <inheritdoc/>
        public override string ToString( ) => LLVMNative.MarshalMsg( LLVMNative.DIDescriptorAsString( MetadataHandle ) );

        /// <summary>Replace all uses of this descriptor with another</summary>
        /// <param name="context">Context to use for replacement</param>
        /// <param name="other">New descriptor to replace this one with</param>
        public void ReplaceAllUsesWith( Context context, Descriptor other )
        {
            LLVMNative.DiDescriptorReplaceAllUsesWith( context.ContextHandle, MetadataHandle, other.MetadataHandle );
        }

        internal LLVMMetadataRef MetadataHandle { get; }

        /// <summary>Empty Descriptor</summary>
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diexpression"/></summary>
    public class Expression : Descriptor
    {
        internal Expression( LLVMMetadataRef handle )
            : base( handle )
        {
        }

    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diglobalvariable"/></summary>
    public class GlobalVariable : Descriptor
    {
        internal GlobalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsGlobalVariable( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilocalvariable"/></summary>
    public class LocalVariable : Descriptor
    {
        internal LocalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsVariable( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dienumerator"/></summary>
    public class Enumerator : Descriptor
    {
        internal Enumerator( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsExpression( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubrange"/></summary>
    public class Subrange : Descriptor
    {
        internal Subrange( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubrange( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>Base class for all Debug info scopes</summary>
    public class Scope : Descriptor
    {
        internal Scope( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsScope( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dicompileunit"/></summary>
    public class CompileUnit : Scope
    {
        internal CompileUnit( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsCompileUnit( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#difile"/></summary>
    public class File : Scope
    {
        internal File( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsFile( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilexicalblock"/></summary>
    public class LexicalBlock : Scope
    {
        internal LexicalBlock( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsLexicalBlock( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilexicalblockfile"/></summary>
    public class LexicalBlockFile : Scope
    {
        internal LexicalBlockFile( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsLexicalBlockFile( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dinamespace"/></summary>
    public class Namespace : Scope
    {
        internal Namespace( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsNameSpace( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubprogram"/></summary>
    public class SubProgram : Scope
    {
        internal SubProgram( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubprogram( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>Base class for Debug info types</summary>
    public class Type : Scope
    {
        internal Type( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

        public DebugInfoFlags Flags => ( DebugInfoFlags )LLVMNative.DITypeGetFlags( MetadataHandle );
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dibasictype"/></summary>
    public class BasicType : Type
    {
        internal BasicType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsBasicType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diderivedtype"/></summary>
    public class DerivedType : Type
    {
        internal DerivedType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsDerivedType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }
    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dicompositetype"/></summary>
    public class CompositeType : Type
    {
        internal CompositeType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsCompositeType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

    }

    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubroutinetype"/></summary>
    public class SubroutineType : CompositeType
    {
        internal SubroutineType( LLVMMetadataRef handle )
            : base( handle )
        {
            if( !LLVMNative.DIDescriptorIsSubroutineType( handle ) )
                throw new ArgumentException( "Invalid handle" );
        }

    }
}
