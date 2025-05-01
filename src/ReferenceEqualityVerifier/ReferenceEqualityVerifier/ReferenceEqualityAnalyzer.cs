using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ReferenceEqualityVerifier
{
    /// <summary>Analyzer to perform the checks for use of reference equality when <see cref="IEquatable{T}"/> should be used</summary>
    /// <remarks>
    /// This analyzer helps in reducing pain in transition from the legacy "cached/interned" managed wrappers approach to
    /// dealing with LLVM handles. That approach proved problematic for a number of reasons and is no longer used. This,
    /// analyzer helps to identify places where reference equality is used but value equality should be used. Reference
    /// equality is rarely ever the correct intended use.
    /// </remarks>
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public class ReferenceEqualityAnalyzer
        : DiagnosticAnalyzer
    {
        private const string RelevantNamespaceName = "Ubiquity.NET.Llvm";

        /// <summary>Diagnostics supported by this analyzer</summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.AllDiagnostics;

        /// <summary>Initializes the analyzer to detect potentially incorrect use</summary>
        /// <param name="context">Compiler provided context for initialization</param>
        public override void Initialize( AnalysisContext context )
        {
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();
            context.RegisterOperationAction( BinaryOpAction, OperationKind.Binary );
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification="No loss of information, exception is converted to a diagnostic")]
        private void BinaryOpAction( OperationAnalysisContext context )
        {
            try
            {
                if(!(context.Operation is IBinaryOperation op))
                {
                    throw new InvalidOperationException( "Unknown case; non-binary operation..." );
                }

                if(op.OperatorKind == BinaryOperatorKind.Equals || op.OperatorKind == BinaryOperatorKind.NotEquals)
                {
                    // comparisons to null literal are OK, intent is clear and explicit...
                    if(IsNullLiteral( op.LeftOperand ) || IsNullLiteral( op.RightOperand ))
                    {
                        return;
                    }

                    if(op.SemanticModel is null)
                    {
                        // no semantic model - nothing to do here...
                        return;
                    }

                    var lht = op.SemanticModel.GetTypeInfo(op.LeftOperand.Syntax).Type;
                    var rht = op.SemanticModel.GetTypeInfo(op.RightOperand.Syntax).Type;
                    if(lht is null || rht is null)
                    {
                        // Incomplete syntax is handled as OK for this analyzer.
                        // Other parts of the compilation/Analyzers can complain as needed
                        // but it should not break this analyzer!
                        return;
                    }

                    if(!IsOneTypeOfInterest( lht, rht ))
                    {
                        return;
                    }

                    // if the types of the operands are Equatable then a diagnostic is reported.
                    if(AreEquatable( lht, rht ))
                    {
                        context.ReportDiagnostic( Diagnostic.Create( Diagnostics.RefEqualityWhenEquatable, op.Syntax.GetLocation(), op.LeftOperand.Syntax, op.RightOperand.Syntax ) );
                    }
                }
            }
            catch(Exception ex)
            {
                context.ReportDiagnostic( Diagnostic.Create( Diagnostics.RefEqualityInternalError, context.Operation.Syntax.GetLocation(), ex.Message ) );
            }
        }

        // tests if at least one of the types is in the namespace of interest
        // This analyzer is intentionally NOT general purpose. It is ONLY focused
        // on the LLVM Wrapper types and their use - nothing else.
        private static bool IsOneTypeOfInterest( ITypeSymbol lht, ITypeSymbol rht )
        {
            return GetNamespaceNames( lht ).Contains( RelevantNamespaceName )
                || GetNamespaceNames( rht ).Contains( RelevantNamespaceName );
        }

        static IEnumerable<string> GetNamespaceNames( ITypeSymbol sym )
        {
            return from part in sym.ContainingNamespace.ToDisplayParts()
                   where part.Kind != SymbolDisplayPartKind.Punctuation
                      && part.Symbol != null
                   select part.Symbol!.ToString();
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
            return IsImplicitlyCastableTo( lht, typeSymbol )
                || IsImplicitlyCastableTo( rht, typeSymbol );
        }

        private static bool IsImplicitlyCastableTo( ITypeSymbol derivedType, ITypeSymbol testBaseType )
        {
            for(ITypeSymbol? baseType = derivedType; baseType != null; baseType = baseType.BaseType)
            {
                if(SymbolEqualityComparer.Default.Equals( baseType, testBaseType ))
                {
                    return true;
                }
            }
            return false;
        }

        // NOTE: Nullability of the types is NOT relevant in this context so all comparisons are done without
        //       consideration for the nullable annotation. (That is, `string` and `string?` are the same)
        private static bool IsString( ITypeSymbol t, SemanticModel model )
        {
            return SymbolEqualityComparer.Default.Equals( t, model.Compilation.GetSpecialType( SpecialType.System_String ) );
        }

        private static bool IsNullLiteral( IOperation op )
        {
            return op.ConstantValue.HasValue && op.ConstantValue.Value is null;
        }
    }
}
