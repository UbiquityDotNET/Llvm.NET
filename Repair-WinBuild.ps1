<#
This script applies a patch for LLVM Bug 47160 when building on Windows, where MSVC is used.
https://bugs.llvm.org/show_bug.cgi?id=47160

The history here is that C++17 included an update to the spec for std::is_trivially_copyable, 
CWG 1734 (http://www.open-std.org/jtc1/sc22/wg21/docs/cwg_defects.html#1734) which changed the expected
behavior of is_trivially_copyable for certain  types. This change was incorporated into MVC but not
into Clang or GCC, creating a difference in behavior (see 
https://stackoverflow.com/questions/50720054/is-stdis-trivially-copyable-wrong). Meanwhile, the LLVM
implementation of llvm::is_trivially_copyable had a bug fix applied that included compile time asserts
to verify the portability of their implementation (see 
https://github.com/llvm/llvm-project/commit/776f809be3a4d166dc53e7f75acd5bfac4fdbe0f). When compiled
with MSVC, these compile time asserts fail, because the llvm implementation doesn't match the updated
std implementaiton. The latest Visual Studio tools inlcude MSVC with the updated behavior, so LLVM
builds no longer pass, and several projects have had to work around this (see below). This work around
updates the config-ix.cmake file used when building LLVM to avoid setting the compile time constant
that enables the asserts. Since this only disables asserts and has no change in behavior, it should
be a safe work around. The LLVM bug and proposed fix do not seem to have any traction, so projects
are left to work around this themsleves for now.

--Additional links for context--
Proposed LLVM fix: https://reviews.llvm.org/D86126
Other projects working around the same issue:
    https://github.com/crystal-lang/crystal/pull/9907/files
    https://github.com/microsoft/vcpkg/pull/12884
#>

Push-Location $PSScriptRoot
$oldPath = $env:Path
try {
    Write-Information "Applying patch for LLVM Bug 47160..."
    $path = "llvm-project\llvm\cmake\config-ix.cmake"
    if (!(Test-Path $path)) {
        throw "Unable to locate config-ix.cmake to apply fix for LLVM Bug 47160. See 'https://bugs.llvm.org/show_bug.cgi?id=47160' for details."
    }
    $content = Get-Content -Raw -Path $path
    $content = $content.Replace("HAVE_STD_IS_TRIVIALLY_COPYABLE", "SKIP_STD_IS_TRIVIALLY_COPYABLE")
    Out-File -FilePath $path -InputObject $content -Encoding utf8NoBOM
    Write-Information "Patch for LLVM Bug 471660 applied."
}
finally {
    Pop-Location
    $env:Path = $oldPath
}
