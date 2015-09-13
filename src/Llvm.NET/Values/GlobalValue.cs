namespace Llvm.NET.Values
{
    /// <summary>LLVM Global value </summary>
    public class GlobalValue : Constant
    {
        /// <summary>Visibility of this global value</summary>
        public Visibility Visibility
        {
            get
            {
                return ( Visibility )LLVMNative.GetVisibility( ValueHandle );
            }
            set
            {
                LLVMNative.SetVisibility( ValueHandle, ( LLVMVisibility )value );
            }
        }

        /// <summary>Linkage specification for this symbol</summary>
        public Linkage Linkage
        {
            get
            {
                return ( Linkage )LLVMNative.GetLinkage( ValueHandle );
            }
            set
            {
                LLVMNative.SetLinkage( ValueHandle, ( LLVMLinkage )value );
            }
        }

        /// <summary>Flag to indicate if this is an Unnamed address</summary>
        public bool UnnamedAddress
        {
            get
            {
                return LLVMNative.HasUnnamedAddr( ValueHandle );
            }
            set
            {
                LLVMNative.SetUnnamedAddr( ValueHandle, value );
            }
        }

        /// <summary>Flag to indicate if this is a declaration</summary>
        public bool IsDeclaration => LLVMNative.IsDeclaration( ValueHandle );

        /// <summary>Module containing this global value</summary>
        public Module ParentModule => Type.Context.GetModuleFor( LLVMNative.GetGlobalParent( ValueHandle ) );

        internal GlobalValue( LLVMValueRef valueRef )
            : base( ValidateConversion( valueRef, LLVMNative.IsAGlobalValue ) )
        {
        }
    }

    public static class GlobalValueExtensions
    {
        /// <summary>Visibility of this global value</summary>
        public static T Visibility<T>( this T value, Visibility visibility )
            where T : GlobalValue
        {
            value.Visibility = visibility;
            return value;
        }

        /// <summary>Linkage specification for this symbol</summary>
        public static T Linkage<T>( this T value, Linkage linkage )
            where T : GlobalValue
        {
            value.Linkage = linkage;
            return value;
        }
    }
}
