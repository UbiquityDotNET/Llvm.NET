# Attribution

This template is a modification of the template provided by Mathew Sachin
see: https://github.com/MathewSachin/docfx-tmpl
combined with modifications of the DocFX memberpage template

Changes:
* Dark color theme
* Changed tags for many headers for items rendering really small due to use of h5/h6
* Modified Enum template from member-pages to remove redundant table headers
* Modified Class template from member-pages to remove redundant headers
* Modified Collections template from default to remove redundant headers
* Fixed bug on class namespace name rendering (Was always saying System.Dynamic.ExpandoObject)
* Added Collapse region for Inherited Members list (Initially collapsed)
* Added Collapse region for Inheritance chain (Initially collapsed)
* Added Collapse region for Derived classes list (Initially collapsed)
* Added Collapse region for Implemented interfaces (Initially collapsed)
* Added llvm IR, and EBNF from highlightjs.org as those languages are not part of the docfx.vendor.js by default
* Added custom ANTLR grammar syntax highlighting as that is not part of standard grammars for highlight js
* Added custom Kaleidoscope grammar syntax highlighting as that is not part of standard grammars for highlight js

