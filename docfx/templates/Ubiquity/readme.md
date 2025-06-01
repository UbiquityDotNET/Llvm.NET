# Ubiquity DOCFX template
This template adds support to the syntax highlighting provided by [HightlightJS](https://highlightjs.readthedocs.io/en/latest/supported-languages.html).
The languages added are for ANTLR (Which seems bizarre it isn't already covered
given the esoteric nature of some of the supported languages...) and of course the
`Kaleidoscope` language, which was made up entirely for the purposes of LLVM
tutorials. (No surprise that one isn't supported) [Though it oddly IS supported
directly in the [Local MarkDig based editor](https://github.com/MadsKristensen/MarkdownEditor2022)
used to edit these files... [Go Figure! :shrug: ]

## Theming
This template also updates the theme for SVG image backgrounds to improve
readability of the parse diagrams. It also updates the HighlightJS classifiers
theming to better show various parts of the ANTLR language.

The default theming is limited, especially for the ANTLR language, as it doesn't
support distinction between function definitions and invocations. (Both have
essentially the default text formatting so it's like none is applied). HighlightJS
has rather limited "scopes" and mapping to them for a language like ANTLR4
Lex/Grammar is a challenge. It is NOT an imperative language (but it does generate
to that) but the HighlightJS is mostly focused on those.
