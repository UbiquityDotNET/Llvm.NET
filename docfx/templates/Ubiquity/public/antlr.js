/*
Simple antlr syntax highlighting for Highlight.JS
*/

export function antlr(hljs)
{
    return {
        aliases: ['antlr'],
        keywords: {
            keyword: 'grammar fragment'
        },
        contains: [
            hljs.COMMENT('//', '\\n', { relevance: 0 }),
            {
                scope: 'regexp',
                variants: [
                    { begin: '\\[', end: '\\]' }
                ],
                relevance: 0
            },
            {
                scope: 'string',
                variants: [
                    { begin: '\'', end: '\'' },
                    { begin: '"', end: '"' }
                ],
                relevance: 0
            },
            {
                scope: 'keyword',
                variants: [
                    { begin: 'i\\d+' }
                ],
                relevance: 0
            },
            {
                scope: 'meta',
                variants: [
                    { begin: '{', end: '}' },
                    { begin: '{', end: '}\?' }
                ],
                relevance: 0
            },
            {
                scope: 'number',
                variants: [
                    { begin: '0[xX][a-fA-F0-9]+' },
                    { begin: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
                ],
                relevance: 0
            },
            {
                scope: 'symbol',
                variants: [
                    { begin: '([A-Z][\\w\\-$.]*)' }
                ],
                relevance: 0
            }
        ]
    };
}
