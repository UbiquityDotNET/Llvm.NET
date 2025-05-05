/*
Simple Kaleidoscope syntax highlighting for Highlight.JS
*/

export function Kaleidoscope(hljs)
{
    return {
        aliases: ['Kaleidoscope', 'kls'],
        keywords: {
            $pattern: '[A-Za-z$_][0-9A-Za-z$_]*',
            keyword: 'def extern if then else for in var unary binary',
            built_in: 'putchard printd'
        },
        contains: [
            hljs.COMMENT('#', '\\n', { relevance: 0 }),
            {
                className: 'number',
                variants: [
                    { begin: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
                ],
                relevance: 0
            }
        ]
    };
}
