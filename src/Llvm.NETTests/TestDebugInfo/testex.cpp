namespace outerns
{
    namespace innerns
    {
        class foo
        {
        public:
            foo( int offset );
            int add( int a, int b );

        private:
            int m_offset;
        };
    }
}

static outerns::innerns::foo adder( 3 );

extern "C" int SimpleAdd( int a, int b )
{
    if( a > b )
        throw a;

    return a + b;
}

extern "C" int AdderAdd( int a, int b )
{
    try
    {
        return adder.add( a, b );
    }
    catch( int x )
    {
        return x;
    }
}

outerns::innerns::foo::foo( int offset )
{
    m_offset = offset;
}

int outerns::innerns::foo::add( int a, int b )
{
    return SimpleAdd( a, b ) + m_offset;
}
