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

outerns::innerns::foo::foo( int offset )
{
    m_offset = offset;
}

int outerns::innerns::foo::add( int a, int b )
{
    return a + b + m_offset;
}

extern "C" int TestAdd( int x, int y )
{
    return adder.add( x, y );
}