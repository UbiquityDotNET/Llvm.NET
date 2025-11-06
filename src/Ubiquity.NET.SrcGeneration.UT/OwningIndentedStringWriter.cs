// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.SrcGeneration.UT
{
    // simple owning indented writer using a StringWriter, this is NOT generalized due to the potential
    // confusion over the ownership when an exception occurs during construction of this type. (It isn't
    // actually moved in such a case - but safe/correct handling of that in general is rather complicated.)
    // In a test scenario, an exception in the constructor will crash the test host and treated as a test
    // failure. This is desired behavior, and extremely unlikely to ever occur, so OK in this special case.
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "DUH, it's file scoped" )]
    [ExcludeFromCodeCoverage]
    internal class OwningIndentedStringWriter
        : IndentedTextWriter
    {
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Owned, move semantics, disposed of in Dispose" )]
        public OwningIndentedStringWriter( string? tabString = null )
            : base( new StringWriter( CultureInfo.CurrentCulture ), tabString ?? DefaultTabString )
        {
        }

        protected override void Dispose( bool disposing )
        {
            if(disposing)
            {
                InnerWriter.Dispose();
            }

            base.Dispose( disposing );
        }

        public override string? ToString( )
        {
            return InnerWriter.ToString();
        }
    }
}
