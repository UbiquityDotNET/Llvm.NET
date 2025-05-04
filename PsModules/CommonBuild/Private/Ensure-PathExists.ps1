function Ensure-PathExists
{
    param([Parameter(Mandatory=$true, ValueFromPipeLine)]$path)
    New-Item -Force -ErrorAction SilentlyContinue -Name $path -ItemType Directory | Out-Null
}
