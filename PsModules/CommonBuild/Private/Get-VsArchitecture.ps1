function Get-VsArchitecture()
{
    switch([System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture)
    {
        X86 {'x86'}
        X64 {'x64'}
        Arm {'ARM'}
        Arm64 {'ARM64'}
        default { throw 'Usupported runtime architecture for MSVC'}
    }
}
