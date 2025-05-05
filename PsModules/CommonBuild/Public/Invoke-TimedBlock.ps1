
function Invoke-TimedBlock([string]$activity, [ScriptBlock]$block)
{
<#
.SYNOPSIS
    Invokes a script block with a timer

.PARAMETER activity
    Name of the activity to output as part of Write-Information messages for the timer

.PARAMETER block
    Script block to execute with the timer

.DESCRIPTION
    This will print a start (via Write-Information), start the timer, run the script block stop the timer
    then print a finish message indicating the total time the script block took to run.
#>
    $timer = [System.Diagnostics.Stopwatch]::StartNew()
    Write-Information "Starting: $activity"
    try
    {
        Invoke-Command -ScriptBlock $block
    }
    catch
    {
        # Everything from the official docs to the various articles in the blog-sphere says this isn't needed
        # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
        # information is retained and the error reported will include the correct source file and line number
        # data for the error. Without this, only the error message is retained and the location information is
        # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
        throw
    }
    finally
    {
        $timer.Stop()
        Write-Information "Finished: $activity - Time: $($timer.Elapsed.ToString())"
    }
}
