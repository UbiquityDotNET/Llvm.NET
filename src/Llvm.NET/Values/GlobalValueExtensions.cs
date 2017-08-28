namespace Llvm.NET.Values
{
    /// <summary>Fluent style extensions for modifying properties of a <see cref="GlobalValue"/></summary>
    public static class GlobalValueExtensions
    {
        /// <summary>Visibility of this global value</summary>
        public static T Visibility<T>( this T self, Visibility value )
            where T : GlobalValue
        {
            self.Visibility = value;
            return self;
        }

        /// <summary>Linkage specification for this symbol</summary>
        public static T Linkage<T>( this T self, Linkage value )
            where T : GlobalValue
        {
            self.Linkage = value;
            return self;
        }

        public static T UnnamedAddress<T>( this T self, bool value )
            where T : GlobalValue
        {
            self.UnnamedAddress = value;
            return self;
        }
    }
}
