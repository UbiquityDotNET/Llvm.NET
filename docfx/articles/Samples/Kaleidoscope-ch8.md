# 8. Kaleidoscope: Compiling to Object Code
This tutorial describes how to adapt the Kaleidoscope JIT engine into an Ahead of Time (AOT) compiler
by generating target specific native object files.

## Choosing a target
LLVM has built-in support for cross-compilation. This allows compiling to the architecture of the platform
you run the compiler on or, just as easily, for some other architecture. For the Kaleidoscope tutorial we'll
focus on just the native target the compiler is running on.

LLVM uses a "Triple" string to describe the target used for code generation. This takes the form `<arch><sub>-<vendor>-<sys>-<abi>`
(see the description of the [Triple](xref:Llvm.NET.Triple) type for more details)

Fortunately, it is normally not required to build such strings directly. 


## Code Generation Changes
The changes in code generation are fairly straight forward and consist of the following basic steps.
1. Remove JIT engine support
2. Expose the bit code module generated, so it is available to the "driver".
3. Saving the target machine (since it doesn't come from the JIT anymore)

The changes are shown below in classic 'diff' form since each change doesn't really need a detailed explanation.
(Nothing even remotely close to "Rocket Science" here. 8^) )
```diff
diff --git a/Samples/Kaleidoscope/Chapter7/CodeGenerator.cs b/Samples/Kaleidoscope/Chapter8/CodeGenerator.cs
index 89e9b2ad8..1e7597c78 100644
--- a/Samples/Kaleidoscope/Chapter7/CodeGenerator.cs
+++ b/Samples/Kaleidoscope/Chapter8/CodeGenerator.cs
@@ -3,9 +3,7 @@
 // </copyright>
 
 using System;
-using System.Collections.Generic;
 using System.Linq;
-using System.Runtime.InteropServices;
 using Antlr4.Runtime;
 using Antlr4.Runtime.Misc;
 using Antlr4.Runtime.Tree;
@@ -13,7 +11,6 @@ using Kaleidoscope.Grammar;
 using Kaleidoscope.Runtime;
 using Llvm.NET;
 using Llvm.NET.Instructions;
-using Llvm.NET.JIT;
 using Llvm.NET.Transforms;
 using Llvm.NET.Values;
 
@@ -30,24 +27,24 @@ namespace Kaleidoscope
         , IKaleidoscopeCodeGenerator<Value>
     {
         // <Initialization>
-        public CodeGenerator( DynamicRuntimeState globalState )
+        public CodeGenerator( DynamicRuntimeState globalState, TargetMachine machine )
         {
             RuntimeState = globalState;
             Context = new Context( );
-            JIT = new KaleidoscopeJIT( );
+            TargetMachine = machine;
             InitializeModuleAndPassManager( );
             InstructionBuilder = new InstructionBuilder( Context );
             FunctionPrototypes = new PrototypeCollection( );
-            FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
             NamedValues = new ScopeStack<Alloca>( );
         }
         // </Initialization>
 
         public bool DisableOptimizations { get; set; }
 
+        public BitcodeModule Module { get; private set; }
+
         public void Dispose( )
         {
-            JIT.Dispose( );
             Context.Dispose( );
         }
 
@@ -112,23 +109,18 @@ namespace Kaleidoscope
         {
             return DefineFunction( ( Function )context.Signature.Accept( this )
                                  , context.BodyExpression
-                                 ).Function;
+                                 );
         }
         // </VisitFunctionDefinition>
 
         // <VisitTopLevelExpression>
         public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
         {
-            var proto = new Prototype( $"anon_expr_{AnonNameIndex++}" );
-            var function = GetOrDeclareFunction( proto, isAnonymous: true );
-
-            var (_, jitHandle) = DefineFunction( function, context.expression( ) );
+            var function = GetOrDeclareFunction( new Prototype( $"anon_expr_{AnonNameIndex++}" )
+                                               , isAnonymous: true
+                                               );
 
-            var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( proto.Identifier.Name );
-            var retVal = Context.CreateConstant( nativeFunc( ) );
-            FunctionModuleMap.Remove( function.Name );
-            JIT.RemoveModule( jitHandle );
-            return retVal;
+            return DefineFunction( function, context.expression( ) );
         }
         // </VisitTopLevelExpression>
 
@@ -452,7 +444,8 @@ namespace Kaleidoscope
         private void InitializeModuleAndPassManager( )
         {
             Module = Context.CreateBitcodeModule( );
-            Module.Layout = JIT.TargetMachine.TargetData;
+            Module.TargetTriple = TargetMachine.Triple;
+            Module.Layout = TargetMachine.TargetData;
             FunctionPassManager = new FunctionPassManager( Module );
             FunctionPassManager.AddPromoteMemoryToRegisterPass( )
                                .AddInstructionCombiningPass( )
@@ -514,25 +507,13 @@ namespace Kaleidoscope
         // </GetOrDeclareFunction>
 
         // <DefineFunction>
-        private (Function Function, IJitModuleHandle JitHandle) DefineFunction( Function function, ExpressionContext body )
+        private Function DefineFunction( Function function, ExpressionContext body )
         {
             if( !function.IsDeclaration )
             {
                 throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
             }
 
-            // Destroy any previously generated module for this function.
-            // This allows re-definition as the new module will provide the
-            // implementation. This is needed, otherwise both the MCJIT
-            // and OrcJit engines will resolve to the original module, despite
-            // claims to the contrary in the official tutorial text. (Though,
-            // to be fair it may have been true in the original JIT and might
-            // still be true for the interpreter)
-            if( FunctionModuleMap.Remove( function.Name, out IJitModuleHandle handle ) )
-            {
-                JIT.RemoveModule( handle );
-            }
-
             var basicBlock = function.AppendBasicBlock( "entry" );
             InstructionBuilder.PositionAtEnd( basicBlock );
             using( NamedValues.EnterScope( ) )
@@ -548,7 +529,7 @@ namespace Kaleidoscope
                 if( funcReturn == null )
                 {
                     function.EraseFromParent( );
-                    return (null, default);
+                    return null;
                 }
 
                 InstructionBuilder.Return( funcReturn );
@@ -560,10 +541,7 @@ namespace Kaleidoscope
                 FunctionPassManager.Run( function );
             }
 
-            var jitHandle = JIT.AddModule( Module );
-            FunctionModuleMap.Add( function.Name, jitHandle );
-            InitializeModuleAndPassManager( );
-            return (function, jitHandle);
+            return function;
         }
         // </DefineFunction>
 
@@ -585,18 +563,11 @@ namespace Kaleidoscope
         private readonly DynamicRuntimeState RuntimeState;
         private static int AnonNameIndex;
         private readonly Context Context;
-        private BitcodeModule Module;
         private readonly InstructionBuilder InstructionBuilder;
         private readonly ScopeStack<Alloca> NamedValues;
-        private readonly KaleidoscopeJIT JIT;
-        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap;
         private FunctionPassManager FunctionPassManager;
+        private TargetMachine TargetMachine;
         private readonly PrototypeCollection FunctionPrototypes;
-
-        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
-        /// <returns>Result of evaluating the expression</returns>
-        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
-        private delegate double AnonExpressionFunc( );
         // </PrivateMembers>
     }
 }
```

## Driver changes
The bulk of the work needed to generate object files is in the "driver" application code. The changes
fall into two general categories:

1. Command line argument handling
2. Generating the output files

### Adding Command Line handling
To allow providing a file like a traditional compiler the driver app needs to have some basic
command line argument handling. ("Basic" in this case means truly rudimentary ::Grin:: )
Generally this just gets a viable file path to use for the source code.

[!code-csharp[ProcessArgs](../../../Samples/Kaleidoscope/Chapter8/Program.cs#ProcessArgs)]

### Update Main()
The real work comes in the Main application driver, though there isn't a lot of additional code
here either. The general plan is:
1. Process the arguments to get the path to compile
2. Open the file for reading
3. Create a new target machine from the default triple of the host
4. Build the parser loop - specifying the file as the input source (instead of the default console)
5. Remove the REPL loop ready state console output handling as compilation isn't interactive
6. Once the parsing has completed, verify the module and emit the object file
7. For diagnostics use, also emit the LLVM IR textual form

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter8/Program.cs#Main)]

## Conclusion
That's it - seriously! Very little change was needed, mostly deleting code. Looking at the changes
it should be clear that it is possible to support runtime choice between JIT and full native compilation
instead of deleting the JIT code. (Implementing this feature is "left as an exercise for the reader" ::Grin::)




