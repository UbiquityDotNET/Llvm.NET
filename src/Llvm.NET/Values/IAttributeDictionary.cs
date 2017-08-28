using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Llvm.NET.Values
{
    public interface IAttributeCollection
        : ICollection<AttributeValue>
    {
    }

    /// <summary>Interface to an Attribute Dictionary</summary>
    /// <remarks>
    /// <para>This interface provides a full collection of all the
    /// attributes keyed by the <see cref="FunctionAttributeIndex"/>
    /// </para>
    /// <note>This connceptually corresponds to the functionality of the
    /// LLVM AttributeSet class for Versions prior to 5. (at this
    /// time v5 is not yet released). In 5 the equivalent type is
    /// currently AttributeList. In v5 AttributeSet has no index and
    /// is therefore more properly a set than in the past. To help
    /// remove confusion and satisfy naming rules this is called
    /// a Dictionary as that reflects the use here and fits the
    /// direction of LLVM</note>
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Name is correct" )]
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Name is correct" )]
    public interface IAttributeDictionary
        : IReadOnlyDictionary<FunctionAttributeIndex, IAttributeCollection>
    {
    }

    public interface IAttributeContainer
    {
        Context Context { get; }

        IAttributeDictionary Attributes { get; }
    }
}
