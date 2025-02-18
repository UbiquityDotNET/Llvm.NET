// -----------------------------------------------------------------------
// <copyright file="StringDisposalMarshalerMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Templates
{
    internal static class StringDisposalMarshalerMap
    {
        public static (string Name, string NativeDisposer) LookupMarshaller( StringDisposal disposal )
        {
            return StringMarshalerMap[ disposal ];
        }

        private static readonly IReadOnlyDictionary<StringDisposal, (string Name, string NativeDisposer)> StringMarshalerMap
            = new Dictionary<StringDisposal, (string Name, string NativeDisposer)>
            {
                [ StringDisposal.None ] = ($"{nameof(AnsiStringMarshaller)}", string.Empty),
                [ StringDisposal.DisposeMessage ] = ("DisposeMessageMarshaller", "LLVMDisposeMessage"),
                [ StringDisposal.OrcDisposeMangledSymbol ] = ("OrcDisposeMangledSymbolMarshaller", "LLVMOrcDisposeMangledSymbol"),
                [ StringDisposal.DisposeErrorMessage ] = ("ErrorMessageMarshaller", "LLVMDisposeErrorMessage")
            };
    }
}
