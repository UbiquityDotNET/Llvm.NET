// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Ubiquity.NET.Llvm.Analyzer
{
    /// <summary>Analyzer to perform the checks for use of reference equality when <see cref="IEquatable{T}"/> should be used</summary>
    /// <remarks>
    /// <para>This analyzer is constrained to ONLY types in the <c>Ubiquity.NET.Llvm</c>
    /// </para>
    /// <para>This analyzer helps in reducing pain in transition from the legacy "cached/interned" managed wrappers approach to
    /// dealing with LLVM handles. That approach proved problematic for a number of reasons and is no longer used. This,
    /// analyzer helps to identify places where reference equality is used but value equality should be used. Reference
    /// equality is rarely ever the correct intended use.</para>
    /// <note type="important">
    /// There is no "FIX" for this, it identifies a potential problem that requires clarification. There are two possible
    /// cases that are unclear or potentially incorrect as written:<br/>
    /// 1) The comparison is intended to use reference equality<br/>
    ///     a) The comparison should change to explicitly use <see cref="object.ReferenceEquals(object, object)"/><br/>
    /// 2) The comparison is intended to use value equality<br/>
    ///     b) The comparison should change to explicitly use <see cref="object.Equals(object, object)"/>.<br/>
    ///
    /// There are intentionally, no fixes, and no means to suppress this message - it identifies an ambiguity of
    /// intent and the means to suppress it is to make the intent explicit by changing the code.
    /// </note>
    /// </remarks>
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public class ReferenceEqualityAnalyzer
        : DiagnosticAnalyzer
    {
        private const string RelevantNamespaceName = "Ubiquity.NET.Llvm";

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.ReferenceEqualityAnalyzer;

        /// <summary>Initializes the analyzer to detect potentially incorrect use</summary>
        /// <param name="context">Compiler provided context for initialization</param>
        public override void Initialize( AnalysisContext context )
        {
            // ignore generated code
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();
            context.RegisterOperationAction( BinaryOpAction, OperationKind.Binary );
        }

        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "No loss of information, exception is converted to a diagnostic" )]
        private void BinaryOpAction( OperationAnalysisContext context )
        {
            try
            {
                if(context.Operation is not IBinaryOperation op)
                {
                    throw new InvalidOperationException( "Unknown case; non-binary operation..." );
                }

                if(op.SemanticModel is null)
                {
                    // no semantic model - nothing to do here...
                    return;
                }

                // operation of interest?
                if(op.OperatorKind != BinaryOperatorKind.Equals && op.OperatorKind != BinaryOperatorKind.NotEquals)
                {
                    return;
                }

                // comparisons to null literal are OK, intent is clear and explicit...
                if(IsNullLiteral( op.LeftOperand ) || IsNullLiteral( op.RightOperand ))
                {
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

                // If either one is a value type, not a relevant comparison for this analyzer
                // NOTE: Mixed types is a syntax error but not one reported by this analyzer.
                if(lht.IsValueType || rht.IsValueType)
                {
                    return;
                }

                // is at least one of the types declared in the namespace of interest for this analyzer?
                if(!IsDeclaredInNamespace( lht, RelevantNamespaceName ) && !IsDeclaredInNamespace( rht, RelevantNamespaceName ))
                {
                    return;
                }

                // if the types of the operands are Equatable then a diagnostic is reported.
                if(AreEquatable( lht, rht ))
                {
                    context.ReportDiagnostic( Diagnostic.Create( Diagnostics.RefEqualityWhenEquatable, op.Syntax.GetLocation(), op.LeftOperand.Syntax, op.RightOperand.Syntax ) );
                }
            }
            catch(Exception ex)
            {
                context.ReportDiagnostic( Diagnostic.Create( Diagnostics.InternalError, context.Operation.Syntax.GetLocation(), ex.Message ) );
            }
        }

        private static bool IsDeclaredInNamespace( ITypeSymbol typeSym, string namespaceName )
        {
            return GetNamespaceNames( typeSym ).Contains( namespaceName );
        }

        // Get the namespace names as a sequence starting with the root.
        // The names are a full form of each "stage of the namespace.
        // EX: `a.b.c` becomes the sequence:
        //    'a', 'a.b`, 'a.b.c'
        // This allows for easy namespace Hierarchy checks.
        private static IEnumerable<string> GetNamespaceNames( ITypeSymbol sym )
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
        //       consideration for the nullable annotation. (That is, `Thing` and `Thing?` are considered the same)
        private static bool AreEquivalent( ITypeSymbol typeSymbol, ITypeSymbol lht, ITypeSymbol rht )
        {
            return IsDerivedFrom( lht, typeSymbol )
                || IsDerivedFrom( rht, typeSymbol );
        }

        private static bool IsDerivedFrom( ITypeSymbol derivedType, ITypeSymbol testBaseType )
        {
            if(testBaseType.TypeKind == TypeKind.Interface)
            {
                return derivedType.AllInterfaces.Contains( testBaseType, SymbolEqualityComparer.Default );
            }
            else
            {
                for(ITypeSymbol? baseType = derivedType; baseType != null; baseType = baseType.BaseType)
                {
                    if(SymbolEqualityComparer.Default.Equals( baseType, testBaseType ))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsNullLiteral( IOperation op )
        {
            return op.ConstantValue.HasValue && op.ConstantValue.Value is null;
        }
    }
}
