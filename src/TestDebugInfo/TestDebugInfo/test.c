struct foo
{
    int a;
    int b[0];
};

void copy( struct foo src, struct foo* pDst )
{
    *pDst = src;
}