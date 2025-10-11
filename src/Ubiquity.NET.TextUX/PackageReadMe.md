# Ubiquity.NET.TextUX
Common Text based (console) UX support. This provides a number of support classes for
Text based UI/UX. This is generally only relevant for console based apps.

## Supported Functionality
`IDiagnosticReporter` interface is at the core of the UX. It is similar in many ways to many
of the logging interfaces available. The primary distinction is with the ***intention*** of
use. `IDiagnosticReporter` specifically assumes the use for UI/UX rather than a
debugging/diagnostic log.

### Messages
All messages for the UX use a simple immutable structure to store the details of a message
represented as `DiagnosticMessage`.

### Pre-Built Reporters
There are a few pre-built implementation of the `IDiagnosticReporter` interface.
* `TextWriterReporter`
    * Base class for writing UX to a `TextWriter`
* `ConsoleReporter`
    * Reporter that reports errors to `Console.Error` and all other errors to `Console.Out`
* `ColoredConsoleReporter`
    * `ConsoleReporter` that colorizes output using ANSI color codes
        * Colors are customizable, but contains a common default
