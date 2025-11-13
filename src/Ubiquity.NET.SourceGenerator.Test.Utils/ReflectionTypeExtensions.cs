// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

namespace Ubiquity.NET.SourceGenerator.Test.Utils
{
    /// <summary>Utility extensions for runtime reflection types</summary>
    public static class ReflectionTypeExtensions
    {
        /// <summary>Simple type test to determine if a type is a basic type</summary>
        /// <param name="t">Type to test</param>
        /// <returns><see langword="true"/> if the type is a basic type; <see langword="false"/> if not</returns>
        /// <remarks>
        /// A basic type for the purposes of this test is one that is a primitive, enum, or string.
        /// </remarks>
        public static bool IsBasicType(this Type t)
        {
            return t.IsPrimitive
                || t.IsEnum
                || t == typeof(string);
        }

#if USE_ISEQUATABLE
        /// <summary>Tests if a type is equatable</summary>
        /// <param name="_">Type to tests</param>
        /// <returns><see langword="true"/> if the type is equatable; <see langword="false"/> if not</returns>
        /// <remarks>
        /// The definition of equatable is not fully understood, so at present this ALWAYS returns false.
        /// However, this acts as a place holder for when it is determined how to accomplish this. Detection
        /// of equatable is a test optimization so does not impact the correctness of a test - only the cost
        /// to run it.
        /// </remarks>
        public static bool IsEquatable(this Type _)
        {
            // TODO: Figure out how to validate that node implements IEquatable<T> where T is
            //       some type in the object hierarchy of 'node'. In a test this would require
            //       full reflection of the type of node to get the full hierarchy and then
            //       test that an implementation of IEquatable<T> exists for that type. Ideally
            //       this should go in deepest hierarchy first ordering as it is most likely
            //       implemented at the lowest layer for an immediate base type. (Though
            //       technically it could be at any level, that's the most likely case so, for
            //       efficiency, test it first)
            // For now, just assume it isn't and skip the optimization...
            return false;
        }
#endif
    }
}
