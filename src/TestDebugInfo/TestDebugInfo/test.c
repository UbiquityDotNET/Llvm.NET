struct foo
{
    int a;
    float b;
    int c[2];
};

struct foo bar = { 1, 2.0, { 3, 4 } };
struct foo baz;

void copy( struct foo src, struct foo* pDst )
{
    *pDst = src;
}

void DoCopy( )
{
    copy( bar, &baz );
}
