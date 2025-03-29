struct foo
{
    int a;
    float b;
    int c[32];
};

struct foo bar = { 1, 2.0, { 3, 4 } };
struct foo baz;

inline static void copy( struct foo const src     // function line here
                       , struct foo* pDst
                       )
{ // function's ScopeLine here
    *pDst = src;
}

//void OtherSig( struct foo const* pSrc, struct foo* pDst )
//{
//    copy( *pSrc, pDst );
//}
//
void DoCopy( )
{
    copy( bar, &baz );
}
