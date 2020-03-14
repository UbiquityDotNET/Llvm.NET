// -----------------------------------------------------------------------
// <copyright file="StringDisposalMarshalerMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Templates
{
    internal static class StringDisposalMarshalerMap
    {
        public static (string Name, string NativeDisposer) LookupMarshaler( StringDisposal disposal )
        {
            return StringMarshalerMap[ disposal ];
        }

        private static readonly IReadOnlyDictionary<StringDisposal, (string Name, string NativeDisposer)> StringMarshalerMap
            = new Dictionary<StringDisposal, (string Name, string NativeDisposer)>
            {
                [ StringDisposal.CopyAlias ] = ("AliasStringMarshaler", string.Empty),
                [ StringDisposal.DisposeMessage ] = ("DisposeMessageMarshaler", "LLVMDisposeMessage"),
                [ StringDisposal.OrcDisposeMangledSymbol ] = ("OrcDisposeMangledSymbolMarshaler", "LLVMOrcDisposeMangledSymbol"),
                [ StringDisposal.DisposeErrorMesage ] = ("ErrorMessageMarshaler", "LLVMDisposeErrorMessage")
            };
    }
}
