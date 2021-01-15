// -----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm.Interop.Properties;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        internal const string LibraryPath = "Ubiquity.NET.LibLlvm";
    }
}
