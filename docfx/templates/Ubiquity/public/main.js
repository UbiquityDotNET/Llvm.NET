export default {
  configureHljs (hljs)
  {
    // Custom ANTLR language support
    hljs.registerLanguage("antlr", function (e) {
        return {
            k:
                'grammar fragment',
            c: [{
                cN: "keyword",
                b: "i\\d+"
            },
            e.C(
                '//', '\\n', { r: 0 }
            ),
            e.QSM,
            e.ASM,
            {
                cN: 'meta',
                v: [
                    { b: '{', e: '}' },
                    { b: '{', e: '}\?' },
                ],
                r: 0
            },
            {
                cN: 'symbol',
                v: [
                    { b: '([A-Z][\\w\\-$.]*)' },
                ]
            },
            {
                cN: 'number',
                v: [
                    { b: '0[xX][a-fA-F0-9]+' },
                    { b: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
                ],
                r: 0
            },
            {
                cN: 'title',
                v: [
                    { b: '([-a-zA-Z$._][\\w\\-$.]*)' },
                ]
            },

            ]
        };
    });

    // Add Kaleidoscope language support for highlightjs.org
    hljs.registerLanguage("Kaleidoscope", function (e) {
        return {
            k: 'def extern if then else for in var unary binary',
            c: [
                e.C(
                    '#', '\\n', { r: 0 }
                ),
                {
                    cN: 'number',
                    v: [
                        { b: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
                    ],
                    r: 0
                }
            ]
        };
    });
  },
}
