using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Microsoft.CodeAnalysis.Testing.ReferenceAssemblies;

namespace ReferenceEqualityVerifier.Test
{
    [TestClass]
    public class ReferenceEqualityVerifierUnitTest
    {
        [TestMethod]
        public async Task EmptySourceAnalyzesClean( )
        {
            var analyzerTest = CreateTestRunner(string.Empty);
            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task CommonDirectCasesReportDiagnostics( )
        {
            var analyzerTest = CreateTestRunner(CommonDirectSource);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(12, 16),
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(20, 16),
                ]
            );

            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task AdvancedInterfacesReportDiagnostics( )
        {
            var analyzerTest = CreateTestRunner(AdvancedItfSource);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(17, 16),
                ]
            );

            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task EquatableToInterfaceReportsDiagnostic( )
        {
            var analyzerTest = CreateTestRunner(EquatableToItfSource);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(17, 16),
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(22, 16)
                ]
            );

            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task CommonBaseEquatableReportsDiagnostic( )
        {
            var analyzerTest = CreateTestRunner(CommonBaseEquatable);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("UNL001", DiagnosticSeverity.Error).WithLocation(16, 16),
                ]
            );

            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task NullCheckDoesNotReportDiagnostics( )
        {
            var analyzerTest = CreateTestRunner(CompareWithNull);
            // no diagnostics expected (ref equality against null is always a ref equality check!)
            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task NoNamespaceDoesNotReportDiagnostics( )
        {
            // Without namespace checking this would trigger an error;
            // Analyzer should be constrained to ONLY the namespace of interest
            var analyzerTest = CreateTestRunner(NoNamespaceEquatable);
            // no diagnostics expected
            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task StructEquatableIsNotReferenceEquality( )
        {
            var analyzerTest = CreateTestRunner(StructEquatable);
            // no diagnostics expected
            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task IncompleteSyntaxDoesNotReportDiagnostics( )
        {
            var analyzerTest = CreateTestRunner(IncompleteSyntax);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("CS1525", DiagnosticSeverity.Error).WithLocation(16,25),
                ]
            );
            await analyzerTest.RunAsync();
        }

        private static AnalyzerTest<DefaultVerifier> CreateTestRunner(string source)
        {
            return new CSharpAnalyzerTest<ReferenceEqualityAnalyzer, DefaultVerifier>{
                TestState = {
                    Sources = { source },
                    ReferenceAssemblies = Net.Net80
                }
            };
        }

        const string CommonDirectSource = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public class Class1
            : IEquatable<Class1>
        {
            public bool Equals( Class1? other ) => true;

            bool Baz(Class1 x)
            {
                return this == x; // OOPS = Reference equality!
            }
        }

        public static class Class1Test
        {
            public static bool Foo(Class1 x)
            {
                return x == Bar; // OOPS - should be .Equals!
            }

            public static bool Foo2(Class1 x)
            {
                return x.Equals(Bar); // Ah, good to go!
            }

            public static bool Foo3()
            {
                return "hello" == "world"; // Strings are special cased, good to go!
            }

            private static readonly Class1 Bar = new();
        }
        """;

        const string AdvancedItfSource = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public interface IBaz
            : IEquatable<IBaz>
        {
        }

        public class Class1
            : IBaz
        {
            public bool Equals( IBaz? other ) => true;

            bool Baz(IBaz x)
            {
                return this == x; // OOPS = Reference equality!
            }
        }
        """;

        const string EquatableToItfSource = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public interface IBaz
        {
        }

        public class Class1
            : IBaz
            , IEquatable<IBaz>
        {
            public bool Equals( IBaz? other ) => true;

            bool Baz(IBaz x)
            {
                return this == x; // OOPS = Reference equality!
            }

            bool Baz2(Class1 x)
            {
                return x == this; // OOPS = Reference equality!
            }
        }
        """;

        const string CompareWithNull = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public interface IBaz
            : IEquatable<IBaz>
        {
        }

        public class Class1
            : IBaz
        {
            public bool Equals( IBaz? other ) => true;

            bool Baz(IBaz x)
            {
                return x == null; // Reference equality is OK here
            }
        }
        """;

        const string CommonBaseEquatable = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public class BaseClass
            : IEquatable<BaseClass>
        {
            public bool Equals( BaseClass? other ) => false;
        }

        public class DerivedClass
            : BaseClass
        {
            bool SomeFunc(DerivedClass other)
            {
                return other == this; // OOPS, ref equality!
            }
        }

        """;

        const string IncompleteSyntax = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public class BaseClass
            : IEquatable<BaseClass>
        {
            public bool Equals( BaseClass? other ) => false;
        }

        public class DerivedClass
            : BaseClass
        {
            bool SomeFunc(DerivedClass other)
            {
                return other == ; // OOPS, not complete syntax...
            }
        }

        """;

       const string NoNamespaceEquatable = """
        using System;

        public class BaseClass
            : IEquatable<BaseClass>
        {
            public bool Equals( BaseClass? other ) => false;
        }

        public class DerivedClass
            : BaseClass
        {
            bool SomeFunc(DerivedClass other)
            {
                return other == this; // OOPS, ref equality!
            }
        }

        """;

        const string StructEquatable = """
        using System;

        namespace Ubiquity.NET.Llvm;

        public record struct Foo(int Bar);

        public class Class1
        {
            bool Baz(Foo y)
            {
                return y == default; // NOT ref equality!
            }
        }
        """;

    }
}
