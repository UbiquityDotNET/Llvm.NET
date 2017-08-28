using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Llvm.NET.BuildTasks
{
    [SuppressMessage( "", "SA1402", Justification = "Closely related to the static validators" )]
    [AttributeUsage( AttributeTargets.Parameter, Inherited = true, AllowMultiple = false )]
    internal sealed class ValidatedNotNullAttribute
        : Attribute
    {
    }

    internal static class Validators
    {
        [ContractAnnotation( "obj:null => halt" )]
        public static void ValidateNotNull( [ValidatedNotNull] this object obj, [InvokerParameterName] string paramName )
        {
            if( obj == null )
            {
                throw new ArgumentNullException( paramName );
            }
        }

        [ContractAnnotation( "str:null => halt" )]
        public static void ValidateNotNullOrWhiteSpace( [ValidatedNotNull] this string str, [InvokerParameterName] string paramName )
        {
            if( string.IsNullOrWhiteSpace( str ) )
            {
                throw new ArgumentException( "Must not be null or whitespace", paramName );
            }
        }

        public static void ValidateRange<T>( this T i, T min, T max, [InvokerParameterName] string paramName )
            where T : struct, IComparable<T>
        {
            if( min.CompareTo( i ) > 0 || i.CompareTo( max ) > 0 )
            {
                throw new ArgumentOutOfRangeException( paramName, i, $"Accepted range: [{min}, {max}]" );
            }
        }

        [ContractAnnotation( "value:null => halt" )]
        public static void ValidatePattern( [ValidatedNotNull] this string value, [RegexPattern] string pattern, [InvokerParameterName] string paramName )
        {
            value.ValidateNotNullOrWhiteSpace( nameof( value ) );
            pattern.ValidateNotNullOrWhiteSpace( nameof( pattern ) );
            var regEx = new System.Text.RegularExpressions.Regex( pattern );
            var match = regEx.Match( value );
            if( !match.Success )
            {
                throw new ArgumentException( $"Value does not conform to required format: {pattern}", paramName );
            }
        }

        [ContractAnnotation( "value:null => halt" )]
        public static void ValidateLength( [ValidatedNotNull] this string value, int min, int max, [InvokerParameterName] string paramName )
        {
            value.ValidateNotNull( paramName );
            if( value.Length < min || value.Length > max )
            {
                throw new ArgumentException( $"Expected string with length in the range [{min}, {max}]", paramName );
            }
        }
    }
}
