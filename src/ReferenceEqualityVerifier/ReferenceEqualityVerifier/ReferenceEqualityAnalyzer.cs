using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ReferenceEqualityVerifier
{
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public class ReferenceEqualityAnalyzer
        : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.AllDiagnostics;

        public override void Initialize( AnalysisContext context )
        {
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(BinaryOpAction, OperationKind.Binary);
        }

        private void BinaryOpAction( OperationAnalysisContext context )
        {
            var op = (IBinaryOperation)context.Operation;
            if(op.OperatorKind == BinaryOperatorKind.Equals || op.OperatorKind == BinaryOperatorKind.NotEquals)
            {
                // comparisons to null literal are OK, intent is clear and explicit...
                if (IsNullLiteral(op.LeftOperand) || IsNullLiteral(op.RightOperand))
                {
                    return;
                }

                var lht = op.SemanticModel?.GetTypeInfo(op.LeftOperand.Syntax).Type;
                var rht = op.SemanticModel?.GetTypeInfo(op.RightOperand.Syntax).Type;
                if(lht is null || rht is null)
                {
                    Debug.Assert(false, "Unknown case types not available");
                    return;
                }

                // if comparing value types, or strings then it's fine; no diagnostic needed
                if( lht.IsValueType || (IsString(lht, context.Compilation) && IsString(rht, context.Compilation)))
                {
                    return;
                }

                // if the types of the operands are Equatable then a diagnostic is reported.
                if(AreEquatable(lht, rht))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Diagnostics.RefEqualityWhenEquatable, op.Syntax.GetLocation(), op.LeftOperand.Syntax, op.RightOperand.Syntax));
                }
            }
        }

        // Determines if two type symbols are equatable
        // NOTE: This isn't completely accurate and might still miss something. If such a case is found, it should
        // be added to the tests for this analyzer and then this method adapted to handle it. At present, this catches
        // the overwhelming majority of cases. [Triggered on several missed cases in the code base!] This is NOT functional
        // enough for a public release but is usable for internal builds at least. For a production release, this would need
        // hardening to handle all cases. With better testing of them all.
        private static bool AreEquatable( ITypeSymbol lht, ITypeSymbol rht )
        {
            var commonItfs = from itf in lht.AllInterfaces.Union(rht.AllInterfaces, SymbolEqualityComparer.Default).Cast<INamedTypeSymbol>()
                             where itf.MetadataName == "IEquatable`1"
                             where AreEquivalent(itf.TypeArguments[0], lht, rht)
                             select itf;

            return commonItfs.Any();
        }

        // NOTE: Nullability of the source types is NOT relevant in this context so all comparisons are done without
        //       consideration for the nullable annotation. (That is, `Thing` and `Thing?` are the same)
        //
        // In particular this currently looks ONLY for explicit IEquatable<T> where T is explicitly rht or lht. That is,
        // it currently does not consider implicit casting and equivalences where one side implements or is derived
        // from the type argument to IEquality<T>.
        private static bool AreEquivalent( ITypeSymbol typeSymbol, ITypeSymbol lht, ITypeSymbol rht )
        {
            if (IsImplicitlyCastableTo(lht, typeSymbol))
            {
                return true;
            }

            if (IsImplicitlyCastableTo(rht, typeSymbol))
            {
                return true;
            }

            return false;
        }

        private static bool IsImplicitlyCastableTo( ITypeSymbol derivedType, ITypeSymbol testBaseType )
        {
            for(ITypeSymbol? baseType = derivedType; baseType != null; baseType = baseType.BaseType)
            {
                if(SymbolEqualityComparer.Default.Equals(baseType, testBaseType))
                {
                    return true;
                }
            }
            return false;
        }

        // NOTE: Nullability of the types is NOT relevant in this context so all comparisons are done without
        //       consideration for the nullable annotation. (That is, `string` and `string?` are the same)
        private static bool IsString(ITypeSymbol t, Compilation compilation)
        {
            return SymbolEqualityComparer.Default.Equals(t, compilation.GetSpecialType(SpecialType.System_String));
        }

        private static bool IsNullLiteral(IOperation op)
        {
            return op.ConstantValue.HasValue && op.ConstantValue.Value is null;
        }
    }
}
