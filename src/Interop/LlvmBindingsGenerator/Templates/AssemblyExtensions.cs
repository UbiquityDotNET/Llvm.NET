// -----------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace LlvmBindingsGenerator.Templates
{
    internal static class AssemblyExtensions
    {
        public static string GetAssemblyInformationalVersion( this Assembly asm )
        {
            var attr = asm.GetCustomAttribute< AssemblyInformationalVersionAttribute>();
            return attr.InformationalVersion;
        }
    }
}
