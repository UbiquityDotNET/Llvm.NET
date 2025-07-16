#
# This is a PowerShell Unit Test file.
# You need a unit test framework such as Pester to run PowerShell Unit tests.
# You can download Pester from https://go.microsoft.com/fwlink/?LinkID=534084
#

# Pester uses Bizzare "keywords" that despite all attempts otherwise, defy logical explanations
# of the metphors behind them. This makes it VERY difficult to use/understand. Fortunately they
# are all just Powershell functions and thus a PS alias can help make more sense of them.
#Set-Alias Test-Block It -Scope Script
#Set-Alias Test-Set Describe -Scope Script
#Set-Alias Test-Group Context -Scope Script
# while this technically works for runtime builds. Tools that search for tests like a test explorer
# in an IDE are often HARD CODED to use the Pester names and will fail if aliasing is used...
#
# Oh, and then the test adapter assumes legacy .NET framework and NOT current Powershell 7
# so it fails to even load the pester module (Even if the latest version is already installed).
# [Sigh..., what a mess...]
#
# Sounded like a good idea at the time, but really, Simpler to just test things manually...
#
BeforeAll {
    Import-Module "$PSScriptRoot\RepoBuild.psd1"
}

Describe "Get-Function" {
    Context "Function Exists" {
        It "Should Return" {
            #Get-Function | Should -Be 'Testing 1,2,3'
        }
    }
}
