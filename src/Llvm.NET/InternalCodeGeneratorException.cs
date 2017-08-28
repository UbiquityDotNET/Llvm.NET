using System;

namespace Llvm.NET
{
    /// <summary>Exception generated when the internal state of the code generation cannot proceed due to an internal error</summary>
    public class InternalCodeGeneratorException : Exception
    {
        public InternalCodeGeneratorException( )
        {
        }

        public InternalCodeGeneratorException( string message )
            : base( message )
        {
        }

        public InternalCodeGeneratorException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}
