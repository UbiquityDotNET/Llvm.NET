/*
Simple Kaleidoscope syntax highlighting for Highlight.JS
*/

export function Kaleidoscope(hljs)
{
    const NUMBER = {
        className: 'number',
        variants: [
            { begin: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
        ],
        relevance: 0
    };

    return
    {
        aliases: ['kaleidoscope', 'kls']
        keywords: [
            'def',
            'extern',
            'if',
            'then',
            'else',
            'for',
            'in',
            'var',
            'unary',
            'binary'
        ],
        contains: [
            hljs.Comment('#', '$', { relevance: 0 }),
            NUMBER
        ]
    };
}
