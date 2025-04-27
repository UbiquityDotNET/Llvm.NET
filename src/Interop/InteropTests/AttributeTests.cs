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
            LazyEncodedString name = new("custom");
            LazyEncodedString value = new("custom value");

            LLVMAttributeRef attribValue;

            using var memName = name.Pin();
            using var memValue = value.Pin();
            unsafe
            {
                attribValue = LLVMCreateStringAttribute(ctx, (byte*)memName.Pointer, (uint)name.NativeStrLen, (byte*)memValue.Pointer, (uint)value.NativeStrLen);
            }

            Assert.IsFalse(attribValue.IsNull);

            // LibLLVM should support Custom attributes as well.
            LibLLVMAttributeInfo info;
            LazyEncodedString attribName = new("custom");
            using var mem = attribName.Pin();
            unsafe
            {
                using LLVMErrorRef errorRef = LibLLVMGetAttributeInfo((byte*)mem.Pointer, attribName.NativeStrLen, &info);
                errorRef.ThrowIfFailed();
            }

            Assert.AreEqual(LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, info.ArgKind);
        }

        [TestMethod]
        public void AttributeListAttainable( )
        {
            size_t len = LibLLVMGetNumKnownAttribs();
            unsafe
            {
                byte** ppData = stackalloc byte*[len];
                using LLVMErrorRef errorRef = LibLLVMGetKnownAttributeNames(len, ppData);
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
