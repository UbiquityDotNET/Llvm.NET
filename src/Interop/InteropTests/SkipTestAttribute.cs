using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    /// <summary>Attribute to mark tests as ignored before completion of development</summary>
    /// <remarks>
    /// This allows creation of tests that can at least compile but are not yet complete (or
    /// even started yet). Such tests act as place-holders for formal testing not yet complete.
    /// These are easier to see in code than a completely missing test with a random TODO or
    /// work item created somewhere (if you are lucky). Thus tests should be created and intent
    /// documented even if they aren't implemented yet (as the underlying code under test is still
    /// taking shape).
    /// </remarks>
    public sealed class SkipTestMethodAttribute
        : TestMethodAttribute
    {
        public override TestResult[] Execute( ITestMethod testMethod )
        {
            return [new() { Outcome = UnitTestOutcome.Ignored }];
        }
    }
}
