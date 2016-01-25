extern int GetANumber( );

inline int InlineDepth2( int inlineDepth2Arg1, int inlineDepth2Arg2 )
{
    int inlineDepth2Local1 = inlineDepth2Arg1 + GetANumber( );
    return inlineDepth2Local1 + inlineDepth2Arg2;
}

inline int InlineDepth1( int inlineDepth1Arg1, int inlinDepth1Arg2 )
{
    int inlineDepth1Local1 = inlinDepth1Arg2 * GetANumber( );
    return InlineDepth2( inlineDepth1Local1, inlineDepth1Arg1);
}