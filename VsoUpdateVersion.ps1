#For details on the general algorithm used here see: https://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.assemblyversionattribute(v=vs.110).aspx 
#The only difference from the AssemblyVersionAttribute algorithm is that this uses UTC for the reference times, thus ensuring that all builds are consistent no matter
#what local the build agent or developer machine is set up for.
$now = [DateTime]::Now
$midnightToday = New-Object DateTime( $now.Year,$now.Month,$now.Day,0,0,0,[DateTimeKind]::Utc)
$basedate = New-Object DateTime(2000,1,1,0,0,0,[DateTimeKind]::Utc)
$buildNum = [int]($now  - $basedate).Days
$buildRevision = [int]((($now - $midnightToday).TotalSeconds) / 2)
Write-Verbose -Verbose ([String]::Format( "##vso[build.updatebuildnumber]3.7.0-alpha_{0}_{1}",$buildNum,$buildRevision))
 