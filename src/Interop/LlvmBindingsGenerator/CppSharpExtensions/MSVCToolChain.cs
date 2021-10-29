// -----------------------------------------------------------------------
// <copyright file="MSVCToolChain.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// NOTE: This source file came from an ILSpy generation from the CppSharpBinaries
// then updated to support VS2019.
//
// Copyright is really from the original OSS CppSharp. However this uses an older version
// and networking was not available at the time this was done. Ultimately, the version
// of CppSharp used here should be updated so this isn't needed.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using CppSharp;

using Microsoft.VisualStudio.Setup.Configuration;

/* using Microsoft.VisualStudio.Setup.Configuration; */

using Microsoft.Win32;

namespace LlvmBindingsGenerator.CppSharpExtensions
{
 // vsVersion is not Hungarian notation...
 // Exception goo, is from original CppSharp library maintained for consistency and eventual replacement with real thing...
#pragma warning disable SA1305 // Field names should not use Hungarian notation
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable CA1031 // Do not catch general exception types

    internal enum VisualStudioVersion
    {
        VS2012 = 11,
        VS2013 = 12,
        VS2015 = 14,
        VS2017 = 17,
        VS2019 = 19,
        Latest = int.MaxValue
    }

    internal static class MSVCToolchain
    {
        public static void DumpSdks()
        {
            List<ToolchainVersion> visualStudioSdks = GetVisualStudioSdks();
            DumpSdks( "Visual Studio", visualStudioSdks );
            List<ToolchainVersion> windowsSdks = GetWindowsSdks();
            DumpSdks( "Windows", windowsSdks );
            List<ToolchainVersion> windowsKitsSdks = GetWindowsKitsSdks();
            DumpSdks( "Windows Kits", windowsKitsSdks );
            List<ToolchainVersion> netFrameworkSdks = GetNetFrameworkSdks();
            DumpSdks( ".NET Framework", netFrameworkSdks );
            List<ToolchainVersion> msbuildSdks = GetMSBuildSdks();
            DumpSdks( "MSBuild", msbuildSdks );
        }

        public static void DumpSdkIncludes( VisualStudioVersion vsVersion = VisualStudioVersion.Latest )
        {
            Console.WriteLine( "\nInclude search path (VS: {0}):", vsVersion );
            foreach( string systemInclude in GetSystemIncludes( vsVersion ) )
            {
                Console.WriteLine( "\t" + systemInclude );
            }
        }

        public static CppSharp.Version GetCLVersion( VisualStudioVersion vsVersion )
        {
            CppSharp.Version result = default;
            switch( vsVersion )
            {
            case VisualStudioVersion.VS2012:
                result.Major = 17;
                result.Minor = 0;
                return result;
            case VisualStudioVersion.VS2013:
                result.Major = 18;
                result.Minor = 0;
                return result;
            case VisualStudioVersion.VS2015:
            case VisualStudioVersion.VS2017:
            case VisualStudioVersion.VS2019:
            case VisualStudioVersion.Latest:
                result.Major = 19;
                result.Minor = 10;
                return result;
            default:
                throw new Exception( "Unknown Visual Studio version" );
            }
        }

        public static ToolchainVersion GetVSToolchain( VisualStudioVersion vsVersion )
        {
            if( VSSdks.Value.Count == 0 )
            {
                throw new Exception( "Could not find a valid Visual Studio toolchain" );
            }

            return ( vsVersion == VisualStudioVersion.Latest ) ? VSSdks.Value.Last() : VSSdks.Value.Find( ( ToolchainVersion version ) => (int)version.Version == GetVisualStudioVersion( vsVersion ) );
        }

        public static ToolchainVersion GetWindowsKitsToolchain( VisualStudioVersion vsVersion, out int windowsSdkMajorVer )
        {
            string directory = GetVSToolchain(vsVersion).Directory;
            directory = directory.Substring( 0, directory.LastIndexOf( "\\Common7\\IDE", StringComparison.Ordinal ) );
            string path = Path.Combine(directory, "Common7\\Tools\\VCVarsQueryRegistry.bat");
            windowsSdkMajorVer = 0;
            string kitsRootKey = string.Empty;
            string input = File.ReadAllText(path);
            Match match = Regex.Match(input, "Windows\\\\v([1-9][0-9]*)\\.?([0-9]*)");
            if( match.Success )
            {
                windowsSdkMajorVer = int.Parse( match.Groups[ 1 ].Value, CultureInfo.InvariantCulture );
            }

            match = Regex.Match( input, "KitsRoot([1-9][0-9]*)" );
            if( match.Success )
            {
                kitsRootKey = match.Groups[ 0 ].Value;
            }

            List<ToolchainVersion> windowsKitsSdks = GetWindowsKitsSdks();
            ToolchainVersion result = (!string.IsNullOrWhiteSpace(kitsRootKey))
                                    ? windowsKitsSdks.Find((ToolchainVersion version) => version.Value == kitsRootKey)
                                    : windowsKitsSdks.Last();

            if( result.Value == null )
            {
                result = windowsKitsSdks.Last();
            }

            return result;
        }

        public static VisualStudioVersion FindVSVersion( VisualStudioVersion vsVersion )
        {
            if( vsVersion != VisualStudioVersion.Latest && GetVSToolchain( vsVersion ).IsValid )
            {
                return vsVersion;
            }

            for( VisualStudioVersion visualStudioVersion = VisualStudioVersion.VS2019; visualStudioVersion >= VisualStudioVersion.VS2012; visualStudioVersion-- )
            {
                vsVersion = FindVSVersion( visualStudioVersion );
                if( vsVersion != VisualStudioVersion.Latest )
                {
                    return vsVersion;
                }
            }

            return VisualStudioVersion.Latest;
        }

        public static List<string> GetSystemIncludes( VisualStudioVersion vsVersion )
        {
            string text = GetVSToolchain(vsVersion).Directory;
            if( Path.GetFileName( text ) == "IDE" )
            {
                string directoryName = Path.GetDirectoryName(text);
                if( Path.GetFileName( directoryName ) == "Common7" )
                {
                    text = Path.GetDirectoryName( directoryName );
                }
            }

            return GetSystemIncludes( vsVersion, text );
        }

        public static List<ToolchainVersion> GetNetFrameworkSdks()
        {
            List<ToolchainVersion> toolchainsFromSystemRegistry = GetToolchainsFromSystemRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework", "InstallRoot", RegistryView.Registry32);
            if( toolchainsFromSystemRegistry.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework", "InstallRoot", RegistryView.Registry64 ) );
            }

            toolchainsFromSystemRegistry.Sort( ( ToolchainVersion v1, ToolchainVersion v2 ) => (int)( v1.Version - v2.Version ) );
            return toolchainsFromSystemRegistry;
        }

        public static List<ToolchainVersion> GetMSBuildSdks()
        {
            List<ToolchainVersion> toolchainsFromSystemRegistry = GetToolchainsFromSystemRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions", "MSBuildToolsPath", RegistryView.Registry32);
            if( toolchainsFromSystemRegistry.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions", "MSBuildToolsPath", RegistryView.Registry64 ) );
            }

            toolchainsFromSystemRegistry.Sort( ( ToolchainVersion v1, ToolchainVersion v2 ) => (int)( v1.Version - v2.Version ) );
            return toolchainsFromSystemRegistry;
        }

        public static List<ToolchainVersion> GetWindowsSdks()
        {
            List<ToolchainVersion> toolchainsFromSystemRegistry = GetToolchainsFromSystemRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows", "InstallationFolder", RegistryView.Registry32);
            if( toolchainsFromSystemRegistry.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows", "InstallationFolder", RegistryView.Registry64 ) );
            }

            toolchainsFromSystemRegistry.Sort( ( ToolchainVersion v1, ToolchainVersion v2 ) => (int)( v1.Version - v2.Version ) );
            return toolchainsFromSystemRegistry;
        }

        public static List<ToolchainVersion> GetWindowsKitsSdks()
        {
            List<ToolchainVersion> toolchainsFromSystemRegistryValues = GetToolchainsFromSystemRegistryValues("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows Kits\\Installed Roots", "KitsRoot", RegistryView.Registry32);
            if( toolchainsFromSystemRegistryValues.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistryValues.AddRange( GetToolchainsFromSystemRegistryValues( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows Kits\\Installed Roots", "KitsRoot", RegistryView.Registry64 ) );
            }

            toolchainsFromSystemRegistryValues.Sort( ( ToolchainVersion v1, ToolchainVersion v2 ) => (int)( v1.Version - v2.Version ) );
            return toolchainsFromSystemRegistryValues;
        }

        public static List<ToolchainVersion> GetVisualStudioSdks()
        {
            List<ToolchainVersion> toolchainsFromSystemRegistry = GetToolchainsFromSystemRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio", "InstallDir", RegistryView.Registry32);
            if( toolchainsFromSystemRegistry.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio", "InstallDir", RegistryView.Registry64 ) );
            }

            toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VCExpress", "InstallDir", RegistryView.Registry32 ) );
            if( toolchainsFromSystemRegistry.Count == 0 && Environment.Is64BitProcess )
            {
                toolchainsFromSystemRegistry.AddRange( GetToolchainsFromSystemRegistry( "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VCExpress", "InstallDir", RegistryView.Registry64 ) );
            }

            GetVs2017Instances( toolchainsFromSystemRegistry );
            toolchainsFromSystemRegistry.Sort( ( ToolchainVersion v1, ToolchainVersion v2 ) => (int)( v1.Version - v2.Version ) );
            return toolchainsFromSystemRegistry;
        }

        public static List<ToolchainVersion> GetToolchainsFromSystemRegistryValues( string keyPath, string matchValue, RegistryView view )
        {
            RegistryHive registryHive = GetRegistryHive(keyPath, out string subKey);
            using( var registryKey = RegistryKey.OpenBaseKey( registryHive, view ) )
            using( RegistryKey registryKey2 = registryKey.OpenSubKey( subKey, writable: false ) )
            {
                var list = new List<ToolchainVersion>();
                if( registryKey2 == null )
                {
                    return list;
                }

                string[] valueNames = registryKey2.GetValueNames();
                foreach( string text in valueNames )
                {
                    if( !text.Contains( matchValue ) )
                    {
                        continue;
                    }

                    if( registryKey2.GetValue( text ) is string text2 )
                    {
                        float result = 0f;
                        Match match = Regex.Match(text2, ".*([1-9][0-9]*\\.?[0-9]*)");
                        if( match.Success )
                        {
                            float.TryParse( match.Groups[ 1 ].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out result );
                        }

                        var toolchainVersion = default(ToolchainVersion);
                        toolchainVersion.Directory = text2;
                        toolchainVersion.Version = result;
                        toolchainVersion.Value = text;
                        ToolchainVersion item = toolchainVersion;
                        list.Add( item );
                    }
                }

                return list;
            }
        }

        private static void DumpSdks( string sku, IEnumerable<ToolchainVersion> sdks )
        {
            Console.WriteLine( "\n{0} SDKs:", sku );
            foreach( ToolchainVersion sdk in sdks )
            {
                Console.WriteLine( "\t({0}) {1}", sdk.Version, sdk.Directory );
            }
        }

        private static int GetVisualStudioVersion( VisualStudioVersion version )
        {
            switch( version )
            {
            case VisualStudioVersion.VS2012:
                return 11;
            case VisualStudioVersion.VS2013:
                return 12;
            case VisualStudioVersion.VS2015:
                return 14;
            case VisualStudioVersion.VS2017:
                return 15;
            case VisualStudioVersion.VS2019:
                return 16;
            default:
                throw new Exception( "Unknown Visual Studio version" );
            }
        }

        private static List<string> GetSystemIncludes( VisualStudioVersion vsVersion, string vsDir )
        {
            if( vsVersion >= VisualStudioVersion.VS2017 )
            {
                return GetSystemIncludesFromVsSetup( vsDir );
            }

            ToolchainVersion windowsKitsToolchain = GetWindowsKitsToolchain(vsVersion, out int windowsSdkMajorVer);
            List<ToolchainVersion> windowsSdks = GetWindowsSdks();
            var list = new List<string>
                {
                    Path.Combine(vsDir, "VC\\include")
                };

            if( windowsSdks.Count == 0 )
            {
                list.Add( Path.Combine( vsDir, "\\VC\\PlatformSDK\\Include" ) );
            }
            else
            {
                list.AddRange( GetIncludeDirsFromWindowsSdks( windowsSdkMajorVer, windowsSdks ) );
            }

            list.AddRange( CollectUniversalCRuntimeIncludeDirs( vsDir, windowsKitsToolchain, windowsSdkMajorVer ) );
            return list;
        }

        private static IEnumerable<string> GetIncludeDirsFromWindowsSdks( int windowsSdkMajorVer, List<ToolchainVersion> windowsSdks )
        {
            ToolchainVersion toolchainVersion = windowsSdks.Find((ToolchainVersion version) => (int)Math.Floor(version.Version) == windowsSdkMajorVer);
            IEnumerable<string> enumerable;
            if( toolchainVersion.Directory == null )
            {
                enumerable = windowsSdks.Select( ( ToolchainVersion w ) => w.Directory ).Reverse();
            }
            else
            {
                IEnumerable<string> enumerable2 = new string[1]
                {
                    toolchainVersion.Directory
                };
                enumerable = enumerable2;
            }

            IEnumerable<string> enumerable3 = enumerable;
            foreach( string item in enumerable3 )
            {
                if( windowsSdkMajorVer >= 8 )
                {
                    string text = Path.Combine(item, "include");
                    IEnumerable<string> enumerable4 = new string[1] { text };
                    if( Directory.Exists( text ) )
                    {
                        enumerable4 = enumerable4.Union( Directory.EnumerateDirectories( text ) );
                    }

                    foreach( string item2 in enumerable4 )
                    {
                        string text2 = Path.Combine(item2, "shared");
                        string text3 = Path.Combine(item2, "um");
                        string text4 = Path.Combine(item2, "winrt");
                        if( Directory.Exists( text2 ) && Directory.Exists( text3 ) && Directory.Exists( text4 ) )
                        {
                            return new string[ 3 ]
                            {
                            text2,
                            text3,
                            text4
                            };
                        }
                    }
                }
                else
                {
                    string text5 = Path.Combine(item, "include");
                    if( Directory.Exists( text5 ) )
                    {
                        return new string[ 1 ] { text5 };
                    }
                }
            }

            return Array.Empty<string>();
        }

        private static IEnumerable<string> CollectUniversalCRuntimeIncludeDirs( string vsDir, ToolchainVersion windowsKitSdk, int windowsSdkMajorVer )
        {
            var list = new List<string>();
            string text = string.Empty;
            string path = Path.Combine(vsDir, "Common7\\Tools\\vsvars32.bat");
            if( File.Exists( path ) )
            {
                string input = File.ReadAllText(path);
                Match match = Regex.Match(input, "INCLUDE=%UniversalCRTSdkDir%(.+)%INCLUDE%");
                if( match.Success )
                {
                    text = match.Groups[ 1 ].Value;
                }
            }

            if( string.IsNullOrWhiteSpace( text ) )
            {
                return list;
            }

            string[] array = text.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach( string text2 in array )
            {
                string text3 = text2.TrimStart('\\');
                int num = text3.IndexOf("%UCRTVersion%", StringComparison.Ordinal);
                if( num >= 0 )
                {
                    string include = text3.Substring(0, num);
                    string path2 = Path.Combine(windowsKitSdk.Directory, include);
                    string dirPrefix = windowsSdkMajorVer + ".";
                    string text4 = (from d in Directory.EnumerateDirectories(path2)
                                    orderby d descending
                                    select d into dir
                                    where Path.GetFileName(dir).StartsWith(dirPrefix, StringComparison.Ordinal)
                                    select Path.Combine(windowsKitSdk.Directory, include, dir)).FirstOrDefault();
                    if( !string.IsNullOrEmpty( text4 ) )
                    {
                        list.Add( Path.Combine( text4, Path.GetFileName( text3 ) ) );
                    }
                }
                else
                {
                    list.Add( Path.Combine( windowsKitSdk.Directory, text3 ) );
                }
            }

            return list;
        }

        private static List<ToolchainVersion> GetToolchainsFromSystemRegistry( string keyPath, string valueName, RegistryView view )
        {
            RegistryHive registryHive = GetRegistryHive(keyPath, out string subKey);
            using( var registryKey = RegistryKey.OpenBaseKey( registryHive, view ) )
            using( RegistryKey registryKey2 = registryKey.OpenSubKey( subKey, writable: false ) )
            {
                var list = new List<ToolchainVersion>();
                if( registryKey2 == null )
                {
                    return list;
                }

                string[] subKeyNames = registryKey2.GetSubKeyNames();
                foreach( string subKeyName in subKeyNames )
                {
                    if( HandleToolchainRegistrySubKey( out var entry, registryKey2, valueName, subKeyName ) )
                    {
                        list.Add( entry );
                    }
                }

                return list;
            }
        }

        private static bool HandleToolchainRegistrySubKey( out ToolchainVersion entry, RegistryKey key, string valueName, string subKeyName )
        {
            entry = default;
            Match match = Regex.Match(subKeyName, "[1-9][0-9]*\\.?[0-9]*");
            if( !match.Success )
            {
                return false;
            }

            string value = match.Groups[0].Value;
            float.TryParse( value, NumberStyles.Number, CultureInfo.InvariantCulture, out float result );
            using( RegistryKey registryKey = key.OpenSubKey( subKeyName ) )
            {
                if( registryKey == null )
                {
                    return false;
                }

                object value2 = registryKey.GetValue(valueName);
                if( value2 == null )
                {
                    return false;
                }

                entry = new ToolchainVersion
                {
                    Version = result,
                    Directory = value2.ToString()
                };
            }

            return true;
        }

        private static RegistryHive GetRegistryHive( string keyPath, out string subKey )
        {
            var result = (RegistryHive)0;
            subKey = null;
            if( keyPath.StartsWith( "HKEY_CLASSES_ROOT\\", StringComparison.Ordinal ) )
            {
                result = RegistryHive.ClassesRoot;
                subKey = keyPath.Substring( 18 );
            }
            else if( keyPath.StartsWith( "HKEY_USERS\\", StringComparison.Ordinal ) )
            {
                result = RegistryHive.Users;
                subKey = keyPath.Substring( 11 );
            }
            else if( keyPath.StartsWith( "HKEY_LOCAL_MACHINE\\", StringComparison.Ordinal ) )
            {
                result = RegistryHive.LocalMachine;
                subKey = keyPath.Substring( 19 );
            }
            else if( keyPath.StartsWith( "HKEY_CURRENT_USER\\", StringComparison.Ordinal ) )
            {
                result = RegistryHive.CurrentUser;
                subKey = keyPath.Substring( 18 );
            }

            return result;
        }

        private static List<string> GetSystemIncludesFromVsSetup( string vsDir )
        {
            var list = new List<string>();
            try
            {
                var val = (SetupConfiguration)new SetupConfigurationClass();
                var val2 = (ISetupConfiguration2)(object)val;
                IEnumSetupInstances val3 = val2.EnumAllInstances();
                var array = (ISetupInstance[])(object)new ISetupInstance[1];
                var regexWinSDK10Version = new Regex("Windows10SDK\\.(\\d+)\\.?");
                int num = default;
                do
                {
                    val3.Next( 1, array, out num );
                    if( num <= 0 )
                    {
                        continue;
                    }

                    var val4 = (ISetupInstance2)array[0];
                    if( val4.GetInstallationPath() != vsDir )
                    {
                        continue;
                    }

                    ISetupPackageReference[] packages = val4.GetPackages();
                    IOrderedEnumerable<ISetupPackageReference> source = from package in packages
                                                                        where package.GetId().Contains("Microsoft.VisualStudio.Component.VC.Tools")
                                                                        orderby package.GetId()
                                                                        select package;
                    if( source.Any() )
                    {
                        string installationPath = val4.GetInstallationPath();
                        string path = installationPath + "\\VC\\Auxiliary\\Build\\Microsoft.VCToolsVersion.default.txt";
                        string str = File.ReadLines(path).ElementAt(0).Trim();
                        list.Add( installationPath + "\\VC\\Tools\\MSVC\\" + str + "\\include" );
                        list.Add( installationPath + "\\VC\\Tools\\MSVC\\" + str + "\\atlmfc\\include" );
                    }

                    IEnumerable<ISetupPackageReference> source2 = packages.Where((ISetupPackageReference package) => package.GetId().Contains("Windows10SDK") || package.GetId().Contains("Windows81SDK") || package.GetId().Contains("Win10SDK_10"));
                    IOrderedEnumerable<ISetupPackageReference> source3 = from sdk in source2
                                                                         where regexWinSDK10Version.Match(sdk.GetId()).Success
                                                                         orderby sdk.GetId()
                                                                         select sdk;
                    IEnumerable<ISetupPackageReference> source4 = source2.Where((ISetupPackageReference sdk) => sdk.GetId().Contains("Windows81SDK"));
                    if( source3.Any() )
                    {
                        ISetupPackageReference val5 = source3.Last();
                        Match match = regexWinSDK10Version.Match(val5.GetId());
                        if( !match.Success )
                        {
                            throw new Exception( "Windows10SDK should not have been detected, something is terribly wrong" );
                        }

                        Environment.SpecialFolder folder = (Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
                        string folderPath = Environment.GetFolderPath(folder);
                        string path2 = Path.Combine(folderPath, "Windows Kits", "10", "include", "10.0." + match.Groups[1].Value + ".0");
                        string text = Path.Combine(path2, "shared");
                        string text2 = Path.Combine(path2, "um");
                        string text3 = Path.Combine(path2, "winrt");
                        string text4 = Path.Combine(path2, "ucrt");
                        if( Directory.Exists( text ) && Directory.Exists( text2 ) && Directory.Exists( text3 ) && Directory.Exists( text4 ) )
                        {
                            list.Add( text );
                            list.Add( text2 );
                            list.Add( text3 );
                            list.Add( text4 );
                        }
                    }
                    else if( source4.Any() )
                    {
                        list.Add( "C:\\Program Files (x86)\\Windows Kits\\8.1\\include\\shared" );
                        list.Add( "C:\\Program Files (x86)\\Windows Kits\\8.1\\include\\um" );
                        list.Add( "C:\\Program Files (x86)\\Windows Kits\\8.1\\include\\winrt" );
                    }

                    return list;
                }
                while( num > 0 );
            }
            catch( COMException ex ) when( ex.HResult == -2147221164 )
            {
            }
            catch( Exception ex2 )
            {
                Console.Error.WriteLine( $"Error 0x{ex2.HResult:x8}: {ex2.Message}" );
            }

            return list;
        }

        private static bool GetVs2017Instances( ICollection<ToolchainVersion> versions )
        {
            try
            {
                var val = (SetupConfiguration)new SetupConfigurationClass();
                var val2 = (ISetupConfiguration2)(object)val;
                IEnumSetupInstances val3 = val2.EnumAllInstances();
                var array = (ISetupInstance[])(object)new ISetupInstance[1];
                int num = default;
                do
                {
                    val3.Next( 1, array, out num );
                    if( num > 0 )
                    {
                        var val4 = (ISetupInstance2)array[0];
                        var toolchainVersion = default(ToolchainVersion);
                        toolchainVersion.Directory = val4.GetInstallationPath() + "\\Common7\\IDE";
                        toolchainVersion.Version = float.Parse( val4.GetInstallationVersion().Remove( 2 ), CultureInfo.InvariantCulture );
                        toolchainVersion.Value = null;
                        ToolchainVersion item = toolchainVersion;
                        versions.Add( item );
                    }
                }
                while( num > 0 );
            }
            catch( COMException ex ) when( ex.HResult == -2147221164 )
            {
                return false;
            }
            catch( Exception ex2 )
            {
                Console.Error.WriteLine( $"Error 0x{ex2.HResult:x8}: {ex2.Message}" );
                return false;
            }

            return true;
        }

        private static readonly Lazy<List<ToolchainVersion>> VSSdks = new Lazy<List<ToolchainVersion>>(GetVisualStudioSdks, isThreadSafe: true);
    }
}
