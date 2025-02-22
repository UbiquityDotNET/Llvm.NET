# Source Generator Unit Tests
The easiest means of debugging a RoslynSourceGenerator is via a Unit test. This unit
test serves as a host for running the generator AND validating it's functionality.

## Current Status
This test covers only the most basic operations, in particular, it covers the
support of proper caching of results. (Critical for incremental generators).