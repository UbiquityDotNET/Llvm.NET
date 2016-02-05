extern "C" int Random();

inline int InlineDepth2( int inlineDepth2Arg1, int inlineDepth2Arg2 )
{
    int xFactor = Random( );
    return inlineDepth2Arg1 + inlineDepth2Arg2 + xFactor;
}

inline int InlineDepth1( int inlineDepth1Arg1, int inlineDepth1Arg2 )
{
    int inlineDepth1Local = inlineDepth1Arg2 + Random( );
    return InlineDepth2( inlineDepth1Arg1, inlineDepth1Local );
}
