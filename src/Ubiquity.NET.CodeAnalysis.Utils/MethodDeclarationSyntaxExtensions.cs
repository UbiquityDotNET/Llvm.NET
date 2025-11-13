// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class to provide extensions for <see cref="MethodDeclarationSyntax"/></summary>
    public static class MethodDeclarationSyntaxExtensions
    {
        // LibraryImportAttribute - intentionally NOT reported as a P/Invoke
        // It uses different qualifiers and, technically, is NOT a P/Invoke
        // signature (It's a generated marshaling function with a nested private
        // P/Invoke using NO marshaling)

        /// <summary>Determines if a method declaration is a P/Invoke</summary>
        /// <param name="self">The <see cref="MethodDeclarationSyntax"/> to test</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> is a P/Invoke declaration or <see langword="false"/> if not</returns>
        /// <remarks>
        /// LibraryImportAttribute is intentionally NOT reported as a P/Invoke. It uses different qualifiers and,
        /// technically, is NOT a P/Invoke signature (It's a marker for a Roslyn source generator. The generated function
        /// contains the marshaling with a nested private P/Invoke using NO marshaling)
        /// </remarks>
        public static bool IsPInvoke(this MethodDeclarationSyntax self)
        {
            return self.IsStatic()
                && self.IsExtern()
                && self.HasAttribute("System.Runtime.InteropServices.DllImportAttribute");
        }
    }
}
