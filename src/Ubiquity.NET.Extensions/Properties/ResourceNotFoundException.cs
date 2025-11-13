// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions.Properties
{
    /// <summary>Exception thornw if a resource is missing</summary>
    /// <remarks>
    /// This is ALWAYS a bug in the application and should not be caught or suppressed in any way.
    /// It indicates that a named resource does not exist, either add the resource or correct the
    /// spelling of the name - NEVER dismiss this.
    /// </remarks>
    [Serializable]
    public class ResourceNotFoundException
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.</summary>
        public ResourceNotFoundException( )
        {
        }

        /// <inheritdoc/>
        public ResourceNotFoundException( string resourceName )
            : base( string.Format( Resources.Culture, Resources.Missing_Resource_Exception_Message_fmt, resourceName ) )
        {
            ResourceName = resourceName;
        }

        /// <inheritdoc/>
        public ResourceNotFoundException( string message, Exception inner )
            : base( message, inner )
        {
        }

        /// <summary>Gets the name of the resource missing</summary>
        public string ResourceName { get; } = string.Empty;
    }
}
