# Ubiquity.NET.CommandLine
Common Text based (console) UX support. This provides a number of support classes for
Text based UI/UXm including command line parsing extensions. This is generally only relevant
for console based apps.

Example Command line parsing:
``` C#
var reporter = new ColoredConsoleReporter(MsgLevel.Information);
if(!ArgsParsing.TryParse<Options>( args, reporter, out Options? options, out int exitCode ))
{
    return exitCode;
}

// ...

// Options is a class that has properties for all parsed commands, arguments and options
// Allowing for validation of them all in context (including each other)
// App can then dispatch behavior based on the commands/options etc... as needed.
// NO ASSUMPTION IS MADE ABOUT THE USE OF COMMANDS NOR THE BEHAVIOR OF THEM. The app
// is entirely in control of how they are used.
```

## Supported Functionality
`IDiagnosticReporter` interface is at the core of the UX. It is similar in many ways to many
of the logging interfaces available. The primary distinction is with the ***intention*** of
use. `IDiagnosticReporter` specifically assumes the use for UI/UX rather than a
debugging/diagnostic log. These have VERY distinct use cases and purposes and generally show
very different information. (Not to mention the overlly complex requirements of
the anti-pattern DI container assumed in `Microsoft.Extensions.Logging`)

### Messages
All messages for the UX use a simple immutable structure to store the details of a message
represented as `DiagnosticMessage`.

### Pre-Built Reporters
There are a few pre-built implementation of the `IDiagnosticReporter` interface.
* `TextWriterReporter`
    * Base class for writing UX to a `TextWriter`
* `ConsoleReporter`
    * Reporter that reports errors to `Console.Error` and all other nessages to
      `Console.Out`
* `ColoredConsoleReporter`
    * `ConsoleReporter` that colorizes output using ANSI color codes
        * Colors are customizable, but contains a common default

