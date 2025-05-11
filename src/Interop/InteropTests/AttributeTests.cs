// -----------------------------------------------------------------------
// <copyright file="DebugRecordTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;
using Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class AttributeTests
    {
        // This tests the underlying behavior of the LLVM and LibLLVM APIs with regard
        // to a custom string attribute. ANY key/value pair is valid for a custom attribute
        // and LLVM does nothing to validate them. (Though a custom target or pass might
        // know more details and provide additional checks)
        [TestMethod]
        public void CustomAttributeTests( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            LazyEncodedString name = new("custom"u8);
            LazyEncodedString value = new("custom value"u8);

            LLVMAttributeRef attribValue = LLVMCreateStringAttribute(ctx, name, value);

            Assert.IsFalse(attribValue.IsNull);

            // LibLLVM should support Custom attributes as well.
            LazyEncodedString attribName = new("custom");
            using LLVMErrorRef errorRef = LibLLVMGetAttributeInfo(attribName, out LibLLVMAttributeInfo info);
            errorRef.ThrowIfFailed();

            Assert.AreEqual(LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, info.ArgKind);
        }

        [TestMethod]
        public void AttributeListAttainable( )
        {
            int len = checked((int)LibLLVMGetNumKnownAttribs());
            unsafe
            {
                byte** ppData = stackalloc byte*[len];
                using LLVMErrorRef errorRef = LibLLVMGetKnownAttributeNames(ppData, (nuint)len);
                errorRef.ThrowIfFailed();

                // make sure conversion is plausible.
                var bldr = ImmutableArray.CreateBuilder<LazyEncodedString>(len);
                for(int i=0; i < len; ++i)
                {
                    bldr.Add(new(ppData[i]));
                }

                var names = bldr.ToImmutable();
            }
        }
    }
}
