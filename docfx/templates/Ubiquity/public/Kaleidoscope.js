/*
Simple Kaleidoscope syntax highlighting for Highlight.JS
*/

export function Kaleidoscope(hljs)
{
    return
    {
        keywords: 'def extern if then else for in var unary binary',
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
