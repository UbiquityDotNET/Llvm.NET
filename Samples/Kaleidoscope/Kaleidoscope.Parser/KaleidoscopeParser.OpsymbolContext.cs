// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class OpsymbolContext
        {
            public char Op => Start.Text[ 0 ];
        }
    }
}
