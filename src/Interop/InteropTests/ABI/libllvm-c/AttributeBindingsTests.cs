using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling;
using Ubiquity.NET.Llvm.Interop.UT;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class AttributeBindingsTests
    {
        [TestMethod]
        public void LibLLVMGetNumKnownAttribsTest( )
        {
            int len = checked((int)LibLLVMGetNumKnownAttribs());
            // Known count of attributes at time of this test
            // Changes are flagged by this test so that updates to consumers or release notes
            // can be made if applicable.
            Assert.AreEqual(KnownAttributes.Count, len);
        }

        [TestMethod]
        public void LibLLVMGetKnownAttributeNamesTest( )
        {
            int len = checked((int)LibLLVMGetNumKnownAttribs());
            unsafe
            {
                byte** ppData = stackalloc byte*[len];
                using LLVMErrorRef errorRef = LibLLVMGetKnownAttributeNames(ppData, (nuint)len);
                errorRef.ThrowIfFailed();

                // make sure conversion is plausible.
                var actualNames = new LazyEncodedString[len];
                for(int i = 0; i < len; ++i)
                {
                    // https://github.com/microsoft/testfx/issues/5543
#pragma warning disable MSTEST0037 // Use proper 'Assert' methods
                    Assert.IsTrue(ppData[i] is not null);
#pragma warning restore MSTEST0037 // Use proper 'Assert' methods

                    var les = LazyEncodedString.FromUnmanaged(ppData[i]);

                    actualNames[i] = LazyEncodedString.FromUnmanaged(ppData[i])!;
                }

                foreach(var name in actualNames)
                {
                    Assert.IsTrue(KnownAttributes.ContainsKey(name), $"Attribute name not known: '{name}'");
                }
            }
        }

        [TestMethod]
        public void LibLLVMAttributeToStringTest( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            LazyEncodedString name = new("custom"u8);
            LazyEncodedString value = "custom value"u8;

            LLVMAttributeRef attribValue = LLVMCreateStringAttribute(ctx, name, value);
            Assert.IsFalse(attribValue.IsNull);

            string result = LibLLVMAttributeToString(attribValue);
            Assert.AreEqual("\"custom\"=\"custom value\"", result);
        }

        [TestMethod]
        public void LibLLVMIsConstantRangeAttributeTest( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();

            // create and validate a known constant range attribute
            LLVMAttributeRef knownConstRangeAttrib = LLVMCreateConstantRangeAttribute(ctx, LLVMGetEnumAttributeKindForName("range"u8), 32, [0], [0x12345678]);
            Assert.IsFalse(knownConstRangeAttrib.IsNull);
            string knownConstRangeAttribAsString = LibLLVMAttributeToString(knownConstRangeAttrib);
            // 0x12345678 = 305419896
            Assert.AreEqual("range(i32 0, 305419896)", knownConstRangeAttribAsString);

            // Create and validate an attribute that is known NOT to be a constant range
            LLVMAttributeRef enumAttrib = LLVMCreateEnumAttribute(ctx, LLVMGetEnumAttributeKindForName("builtin"u8), 0);
            Assert.IsFalse(enumAttrib.IsNull);
            string enumAttribAsString = LibLLVMAttributeToString(enumAttrib);
            Assert.AreEqual("builtin", enumAttribAsString);

            // Now actually test the API in question.
            Assert.IsTrue(LibLLVMIsConstantRangeAttribute(knownConstRangeAttrib));
            Assert.IsFalse(LibLLVMIsConstantRangeAttribute(enumAttrib));
        }

        [SkipTestMethod]
        public void LibLLVMIsConstantRangeListAttributeTest( )
        {
            // At present the creation of a constant range list attribute instance is
            // not possible with the LLVM-C API nor the extended API. It's not entirely
            // clear on what such a thing really is...
            // The only known attribute supporting this type of arg is the "initializes" attribute.

            // TEMP: find a known attribute requiring a constant range list...
            var attrib = ( from kvp in KnownAttributes
                           where kvp.Value.ArgKind == LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_ConstantRangeList
                           select kvp
                         ).FirstOrDefault();

            throw new NotImplementedException();
        }

        [TestMethod]
        public void LibLLVMGetAttributeInfoTest( )
        {
            foreach(var kvp in KnownAttributes.OrderBy(kvp=>kvp.Value.ID))
            {
                LibLLVMGetAttributeInfo( kvp.Key, out LibLLVMAttributeInfo info ).ThrowIfFailed();
                Assert.AreEqual( kvp.Value, info );
                if(info.ID == 0)
                {
                    // ALL attributes with enum ID == 0 should be "string"
                    // [Double checks any potential conflicts with KnownAttributes map]
                    Assert.AreEqual( LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, info.ArgKind );
                }

                ShowKnownAttributesEntry( kvp.Key, info );
            }

            // LibLLVM should support Custom attributes as well.
            using LLVMErrorRef errorRef = LibLLVMGetAttributeInfo("custom"u8, out LibLLVMAttributeInfo customAttribinfo);
            errorRef.ThrowIfFailed();

            Assert.AreEqual(0u, customAttribinfo.ID);
            Assert.AreEqual(LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, customAttribinfo.ArgKind);
            Assert.AreEqual(LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All, customAttribinfo.AllowedOn);
        }

        [TestMethod]
        public void LibLLVMGetAttributeNameFromIDTest( )
        {
            var knownEnumAttribs = from kvp in KnownAttributes
                                   where kvp.Value.ID > 0
                                   select kvp;

            foreach(var kvp in knownEnumAttribs)
            {
                var actualName = LibLLVMGetAttributeNameFromID(kvp.Value.ID);
                Assert.IsNotNull(actualName, $"Name should not be null for ID: {kvp.Value.ID})");
                Assert.AreEqual(kvp.Key, actualName, $"Name for ID: {kvp.Value.ID} should be '{kvp.Key})'");
            }
        }

        private readonly static ImmutableDictionary<LazyEncodedString, LibLLVMAttributeInfo> KnownAttributes
            = new DictionaryBuilder<LazyEncodedString, LibLLVMAttributeInfo> {
                ["no-infs-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["approx-func-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["no-jump-tables"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["denormal-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["less-precise-fpmad"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["no-signed-zeros-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["profile-sample-accurate"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["no-nans-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["unsafe-fp-math"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["use-sample-profile"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                ["no-inline-line-tables"u8] = new(0, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_All),
                // First Enum attribute...
                ["allocalign"u8] = new(1, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["allocptr"u8] = new(2, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["alwaysinline"u8] = new(3, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["builtin"u8] = new(4, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["cold"u8] = new(5, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["convergent"u8] = new(6, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["coro_only_destroy_when_complete"u8] = new(7, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["coro_elide_safe"u8] = new(8, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["dead_on_unwind"u8] = new(9, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["disable_sanitizer_instrumentation"u8] = new(10, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["fn_ret_thunk_extern"u8] = new(11, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["hot"u8] = new(12, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["hybrid_patchable"u8] = new(13, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["immarg"u8] = new(14, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["inreg"u8] = new(15, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["inlinehint"u8] = new(16, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["jumptable"u8] = new(17, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["minsize"u8] = new(18, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["mustprogress"u8] = new(19, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["naked"u8] = new(20, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nest"u8] = new(21, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["noalias"u8] = new(22, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["nobuiltin"u8] = new(23, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nocallback"u8] = new(24, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nocapture"u8] = new(25, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["nocf_check"u8] = new(26, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nodivergencesource"u8] = new(27, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noduplicate"u8] = new(28, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noext"u8] = new(29, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["nofree"u8] = new(30, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noimplicitfloat"u8] = new(31, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noinline"u8] = new(32, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nomerge"u8] = new(33, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noprofile"u8] = new(34, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["norecurse"u8] = new(35, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noredzone"u8] = new(36, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noreturn"u8] = new(37, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nosanitize_bounds"u8] = new(38, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nosanitize_coverage"u8] = new(39, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nosync"u8] = new(40, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["noundef"u8] = new(41, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["nounwind"u8] = new(42, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nonlazybind"u8] = new(43, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nonnull"u8] = new(44, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["null_pointer_is_valid"u8] = new(45, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["optforfuzzing"u8] = new(46, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["optdebug"u8] = new(47, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["optsize"u8] = new(48, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["optnone"u8] = new(49, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["presplitcoroutine"u8] = new(50, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["readnone"u8] = new(51, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["readonly"u8] = new(52, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["returned"u8] = new(53, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["returns_twice"u8] = new(54, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["signext"u8] = new(55, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["safestack"u8] = new(56, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_address"u8] = new(57, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_hwaddress"u8] = new(58, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_memtag"u8] = new(59, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_memory"u8] = new(60, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_numerical_stability"u8] = new(61, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_realtime"u8] = new(62, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_realtime_blocking"u8] = new(63, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_thread"u8] = new(64, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sanitize_type"u8] = new(65, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["shadowcallstack"u8] = new(66, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["skipprofile"u8] = new(67, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["speculatable"u8] = new(68, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["speculative_load_hardening"u8] = new(69, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["ssp"u8] = new(70, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sspreq"u8] = new(71, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sspstrong"u8] = new(72, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["strictfp"u8] = new(73, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["swiftasync"u8] = new(74, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["swifterror"u8] = new(75, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["swiftself"u8] = new(76, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["willreturn"u8] = new(77, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["writable"u8] = new(78, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["writeonly"u8] = new(79, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["zeroext"u8] = new(80, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["byref"u8] = new(81, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["byval"u8] = new(82, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["elementtype"u8] = new(83, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["inalloca"u8] = new(84, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["preallocated"u8] = new(85, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["sret"u8] = new(86, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["align"u8] = new(87, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["allockind"u8] = new(88, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["allocsize"u8] = new(89, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["captures"u8] = new(90, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["dereferenceable"u8] = new(91, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["dereferenceable_or_null"u8] = new(92, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["memory"u8] = new(93, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["nofpclass"u8] = new(94, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["alignstack"u8] = new(95, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["uwtable"u8] = new(96, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["vscale_range"u8] = new(97, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function),
                ["range"u8] = new(98, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_ConstantRange, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return | LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
                ["initializes"u8] = new(99, LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_ConstantRangeList, LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter),
            }.ToImmutable();

        // for a debug build, write the element entries for the "KnownAttributes"
        // This makes it easier to update that map AFTER validation of any changed values.
        [Conditional("DEBUG")]
        private static void ShowKnownAttributesEntry( LazyEncodedString name, LibLLVMAttributeInfo info )
        {
            var flagValues = from val in info.AllowedOn.ToString().Split(',')
                             select $"{nameof(LibLLVMAttributeAllowedOn)}.{val.Trim()}";

            if(info.ID == 1)
            {
                Debug.WriteLine( "// First Enum attribute..." );
            }

            Debug.WriteLine( $"[\"{name}\"u8] = new({info.ID}, LibLLVMAttributeArgKind.{info.ArgKind}, {string.Join( " | ", flagValues )})," );
        }
    }
}
