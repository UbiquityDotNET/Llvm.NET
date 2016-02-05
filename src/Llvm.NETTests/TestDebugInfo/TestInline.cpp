#include "TestInline.h"

extern "C"
{
    int NotInlined( int NotInlinedArg1, int NotInlinedArg2 )
    {
        int NotInlinedLocal = NotInlinedArg1 / Random( );
        return InlineDepth1( NotInlinedLocal, NotInlinedArg2 );
    }
}