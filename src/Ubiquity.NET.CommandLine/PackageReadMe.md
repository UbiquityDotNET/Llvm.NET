# Ubiquity.NET.CommandLine
Command line parsing support extending System.CommandLine to better isolate the parsing
from the app specific validation and execution.

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
