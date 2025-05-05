# Ubiquity DOCFX template
This template adds support to the syntx highlighting provided by [HightlightJS](https://highlightjs.readthedocs.io/en/latest/supported-languages.html).
The languages added are for ANTLR^1^ (Which seems bizarre it isn't already covered given the
esoteric nature of some of the supported languages...) and of course the `Kaleidoscope`
language, which was made up entirely for the purposes of LLVM tutorials. (No surprise that one
isn't supported)

^1^The support for ANTLR is not present. DOCFX officially documents that Highlight JS
extensibility is supported by this. It even has an example. However, that example uses
a language that is built-in to Highlight JS already. It doesn't seem to actually use
this extention point as the Kaleidoscope highlighting never happens ([Local MarkDig
based editor](https://github.com/MadsKristensen/MarkdownEditor2022) will render the
ANTLR with syntax highlighting [That seems to be using PRISM for highlighting though].
But the final docs hosted by DOCFX do not. Thus, this functionality is currently
broken.
