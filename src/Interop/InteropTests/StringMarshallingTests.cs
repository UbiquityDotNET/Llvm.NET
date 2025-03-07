// -----------------------------------------------------------------------
// <copyright file="StringMarshallingTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    // TODO: Test encoding handling
    [TestClass]
    public class StringMarshallingTests
    {
        [TestMethod]
        [SuppressMessage( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "Broken analyzer, suggested API does NOT handle unsafe pointer types" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "It's a pointer - chill" )]
        public void TestConvertToUnmanagedDefault()
        {
            unsafe
            {
                byte* pNativeString = null;
                try
                {
                    pNativeString = ExecutionEncodingStringMarshaller.ConvertToUnmanaged("123");
                    Assert.IsTrue(pNativeString is not null);
                    Assert.AreEqual( (byte)0x31, pNativeString[0]);
                    Assert.AreEqual( (byte)0x32, pNativeString[1]);
                    Assert.AreEqual( (byte)0x33, pNativeString[2]);
                    Assert.AreEqual( 0, pNativeString[3]);
                }
                finally
                {
                    if (pNativeString is not null)
                    {
                        ExecutionEncodingStringMarshaller.Free(pNativeString);
                    }
                }
            }
        }

        [TestMethod]
        [SkipLocalsInit]
        public void TestConvertToManagedDefault()
        {
            unsafe
            {
                byte* nativeBytes = stackalloc byte[4];
                nativeBytes[0] = 0x31;
                nativeBytes[1] = 0x32;
                nativeBytes[2] = 0x33;
                nativeBytes[3] = 0;

                string? managed = ExecutionEncodingStringMarshaller.ConvertToManaged(nativeBytes);
                Assert.IsNotNull(managed);
                Assert.AreEqual( 3, managed.Length);
                Assert.AreEqual("123", managed);
            }
        }

        [TestMethod]
        [SuppressMessage( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "Broken analyzer, suggested API does NOT handle unsafe pointer types" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "It's a pointer - chill" )]
        [SkipLocalsInitAttribute]
        public void TestConVersionToManagedWithInSemantics()
        {
            // simulate the general flow of code used by the LibraryImportAttribute source generator
            unsafe
            {
                ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn marshaller = new();
                try
                {
#pragma warning disable CS9081 // A result of a stackalloc expression of this type in this context may be exposed outside of the containing method
                    // It isn't exposed and this is EXACTLY what the source generator does!
                    marshaller.FromManaged("123", stackalloc byte[ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn.BufferSize]);
#pragma warning restore CS9081 // A result of a stackalloc expression of this type in this context may be exposed outside of the containing method

                    byte* pNativeString = marshaller.ToUnmanaged();
                    Assert.IsTrue(pNativeString is not null);
                    Assert.AreEqual( (byte)0x31, pNativeString[0]);
                    Assert.AreEqual( (byte)0x32, pNativeString[1]);
                    Assert.AreEqual( (byte)0x33, pNativeString[2]);
                    Assert.AreEqual( 0, pNativeString[3]);
                }
                finally
                {
                    marshaller.Free();
                }
            }
        }

        [TestMethod]
        [SuppressMessage( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "Broken analyzer, suggested API does NOT handle unsafe pointer types" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "It's a pointer - chill" )]
        [SkipLocalsInitAttribute]
        public void TestConVersionToManagedWithInSemanticsAllocated()
        {
            // simulate the general flow of code used by the LibraryImportAttribute source generator
            unsafe
            {
                ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn marshaller = new();
                try
                {
                    // use a default span as the input so the size is 0, which forces the marshaller to perform internal allocation
                    // and free is relevant (and really required here)
                    marshaller.FromManaged("123", default);

                    byte* pNativeString = marshaller.ToUnmanaged();
                    Assert.IsTrue(pNativeString is not null);
                    Assert.AreEqual( (byte)0x31, pNativeString[0]);
                    Assert.AreEqual( (byte)0x32, pNativeString[1]);
                    Assert.AreEqual( (byte)0x33, pNativeString[2]);
                    Assert.AreEqual( 0, pNativeString[3]);
                }
                finally
                {
                    marshaller.Free();
                }
            }
        }
    }
}
