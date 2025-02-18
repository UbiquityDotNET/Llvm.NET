using System;
using System.CodeDom.Compiler;

namespace SourceGenerator.Utils
{
    /// <summary>Extension methods for an <see cref="IndentedTextWriter"/></summary>
    public static class IndentedWriterExtensions
    {
        /// <summary>Pushes the indentation level returning an <see cref="IDisposable"/> that restores (pops) it on dispose</summary>
        /// <param name="writer">Writer to indent</param>
        /// <returns><see cref="IDisposable"/> that will restore (pop) the indentation one level when <see cref="IDisposable.Dispose"/> is called</returns>
        /// <remarks>
        /// This is a RAII like operation that simplifies/clarifies indentation updates for an
        /// <see cref="IndentedTextWriter"/>. The return is generally used with a C# 'using' statement
        /// or language equivalent to achieve automatic "pop" behavior.
        /// </remarks>
        public static IDisposable PushIndent(this IndentedTextWriter writer)
        {
            ++writer.Indent;
            return new DisposableAction(() => --writer.Indent);
        }
    }
}
