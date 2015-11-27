namespace Test
{
    struct foo
    {
        int a;
        float b;
        int c[2];
    };

    foo bar = { 1, 2.0, { 0 } };
    foo baz;

    inline static void copy( foo const src     // function line here 
                           , foo* pDst
                           )
    { // function's ScopeLine here
        *pDst = src;
    }

    void OtherSig( foo& rSrc, foo* pDst )
    {
        copy( rSrc, pDst );
    }

    void DoCopy( )
    {
        copy( bar, &baz );
    }
}