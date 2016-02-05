#include "TestInline.h"

extern "C"
{
    int NotInlined( int NotInlinedArg1, int NotInlinedArg2 )
    {
        int NotInlinedLocal = NotInlinedArg1 / GetANumber( );
        return InlineDepth1( NotInlinedLocal, NotInlinedArg2 );
    }
}