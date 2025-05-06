/*
Simple antlr syntax highlighting for Highlight.JS
*/

export function antlr(hljs)
{
    // lexer rules always begin with uppercase character
    const lexerIdent_RE = '\\b[A-Z]' + hljs.UNDERSCORE_IDENT_RE + '\\b';

    // grammar rules always begin with a lowercase character
    const grammarIdent_RE = '\\b[a-z]' + hljs.UNDERSCORE_IDENT_RE + '\\b';

    const semanticPredicate = {
        scope: 'SemanticPredicate',
        begin: '{',
        end: /}\?/,
    };

    const lexerCommand = {
        scope: 'LexerCommand',
        begin: '->',
        beginScope: 'punctuation',
        end: ';',
        endScope: 'punctuation',
        endsParent: true,
        keywords: {
            built_in: 'skip more popMode mode pushMode type channel'
        }
    };

    const lexerRuleRef = {
        scope: 'LexerRuleRef',
        begin: lexerIdent_RE,
        relevance: 0
    };

    const lexerRule = {
        scope: 'LexerRule',
        begin: [lexerIdent_RE, '\\s*', ':', '\\s*'],
        beginScope: {
            1: 'LexerRuleName',
            3: 'punctuation',
        },
        end: ';',
        endScope: 'punctuation',
        relevance: 10,
        contains: [
            semanticPredicate,
            lexerCommand,
            lexerRuleRef,
            hljs.C_LINE_COMMENT_MODE,
            hljs.APOS_STRING_MODE,
            hljs.QUOTE_STRING_MODE,
            hljs.BACKSLASH_ESCAPE,
        ]
    };

    const grammarAltName = {
        begin: "#",
        end: /\s*$/,
        contains: [
            {
                scope: 'GrammarAltName',
                begin: hljs.UNDERSCORE_IDENT_RE,
            }
        ]
    };

    const grammarRuleRef = {
        scope: 'GrammarRuleRef',
        begin: grammarIdent_RE,
        relevance: 0
    };

    const grammarRule = {
        scope: 'GrammarRule',
        begin: [grammarIdent_RE, '\\s*', ':', '\\s*'],
        beginScope: {
            1: "GrammarRuleName",
            3: "punctuation",
        },
        end: ';',
        endScope: 'punctuation',
        relevance: 10,
        contains: [
            grammarAltName,
            semanticPredicate,
            lexerRuleRef,
            grammarRuleRef,
            hljs.C_LINE_COMMENT_MODE,
            hljs.APOS_STRING_MODE,
            hljs.QUOTE_STRING_MODE,
            hljs.C_NUMBER_MODE,
        ]
    };

    return {
        name: "ANTLR V4 (.g4)",
        aliases: ['antlr'],
        keywords: ['grammar', 'fragment', 'skip'],
        classNameAliases: {
            LexerRuleName: 'type',
            LexerRuleRef: 'title.function.invoke',
            GrammarRuleName: 'type',
            GrammarRuleRef: 'title.function.invoke',
            GrammarAltName: 'title.function',
        },
        contains: [
            hljs.C_LINE_COMMENT_MODE,
            lexerRule,
            grammarRule,
        ]
    };
}
