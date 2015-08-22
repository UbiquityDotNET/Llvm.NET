struct foo
{
    int a;
    int b;
};

void copy( foo src, foo* pDst )
{
    *pDst = src;
}