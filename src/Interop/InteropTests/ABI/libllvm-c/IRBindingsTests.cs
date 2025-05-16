using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop.UT;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.IRBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class IRBindingsTests
    {
        [SkipTestMethod]
        public void LibLLVMHasUnwindDestTest( )
        {
            // As of this writing, the only implemented instructions that might contain an unwind dest are a CleanupReturn and
            // CatchSwitchInst, all other instructions result in a 0;
            // TODO: Figure out minimum API calls needed to build a valid case for each type and at least one for the "negative"
            //       then validate the API returns the correct value.
        }
    }
}
