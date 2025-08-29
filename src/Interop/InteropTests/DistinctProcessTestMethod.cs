// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tmds.Utils;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    // see: https://stackoverflow.com/questions/8189657/can-i-force-mstest-to-use-a-new-process-for-each-test-run/77593713#77593713
    // Modified to work in both VS (marked as skipped) and in dotnet.exe
    // I don't pretend to understand why the distinction but as long as dotnet.exe works I can live with the
    // crutch. (Ideally the test infrastructure would have a way to deal with this directly)
    public sealed class DistinctProcessTestMethod
        : TestMethodAttribute
    {
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for OOP execution; exception is captured and reported as a failed test" )]
        public override TestResult[] Execute( ITestMethod testMethod )
        {
            try
            {
                object? obj = Activator.CreateInstance(testMethod.MethodInfo.DeclaringType!);
                var action = testMethod.MethodInfo.CreateDelegate<Action>( obj );
                Executor.Run( action );
                return [ new() { Outcome = UnitTestOutcome.Passed } ];
            }
            catch(TypeInitializationException ex)
            {
                // undocumented; found empirically - if run from VS/IDE type initialization fails with an inner
                // exception of NotSupportedException.
                var result = new TestResult()
                {
                    Outcome = UnitTestOutcome.Ignored,
                    TestFailureException = ex.InnerException
                };

                return [ result ];
            }
            catch(Exception e)
            {
                return [ new() { TestFailureException = e, Outcome = UnitTestOutcome.Failed } ];
            }
        }

        private static readonly FunctionExecutor Executor = new(
            static o =>
            {
                o.StartInfo.RedirectStandardError = true;
                o.OnExit = static p =>
                {
                    if (p.ExitCode != 0)
                    {
                        throw new InvalidOperationException(p.StandardError.ReadToEnd());
                    }
                };
            });
    }
}
