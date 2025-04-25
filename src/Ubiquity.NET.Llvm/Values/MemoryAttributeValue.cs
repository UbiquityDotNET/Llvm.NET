namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Location for memory access attribute</summary>
    public enum MemLocation
    {
        /// <summary>Argument memory</summary>
        ArgMem,

        /// <summary>Memory that is inaccessible via LLVM IR</summary>
        InaccessibleMem,

        /// <summary>Any other memory</summary>
        Other,
    }

    /// <summary>Flags indicating whether a memory access modifies or references memory</summary>
    [Flags]
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    public enum MemAccess
        : byte
    {
        /// <summary>Access does not reference or modify the value stored in memory</summary>
        None = 0,

        /// <summary>The access may reference the value stored in memory</summary>
        Read = 1,

        /// <summary>The access may modify the value stored in memory</summary>
        Write = 2,

        /// <summary>The access may modify or reference the value stored in memory</summary>
        ReadWrite = Read | Write,
    }

    /// <summary>Value argument for <see cref="AttributeKind.Memory"/></summary>
    /// <remarks>
    /// The value is an integer that consists of a bitfield for variations. This,
    /// struct handles the processing of the bit fields.
    /// </remarks>
    public readonly record struct MemoryAttributeValue
    {
        /// <summary>Initializes a new instance of the <see cref="MemoryAttributeValue"/> struct</summary>
        /// <param name="value">ABI value to wrap</param>
        public MemoryAttributeValue(UInt32 value)
        {
            Value = value;
        }

        /// <summary>Initializes a new instance of the <see cref="MemoryAttributeValue"/> struct</summary>
        /// <param name="pairs">Pairs of location and access to initialize this instance with</param>
        public MemoryAttributeValue(IEnumerable<KeyValuePair<MemLocation, MemAccess>> pairs)
        {
            UInt32 fullValue = 0;
            foreach(var kvp in pairs)
            {
                (UInt32 value, UInt32 mask) = GetValueAndMask(kvp.Key, kvp.Value);
                fullValue &= ~mask;
                fullValue |= value;
            }

            Value = fullValue;
        }

        /// <summary>Gets the <see cref="MemAccess"/> for a given <see cref="MemLocation"/></summary>
        /// <param name="index">Location to get the access for</param>
        /// <returns>Access for the current location</returns>
        public MemAccess this[MemLocation index]
        {
            get
            {
                (int shiftVal, UInt32 mask) = GetShiftAndMask(index);
                return (MemAccess)((Value & mask) >> shiftVal);
            }
        }

        /// <summary>Converts a memory attribute value to a raw integral value</summary>
        /// <returns>Raw integral value for the memory attribute</returns>
        public UInt32 ToUInt32() => Value;

        /// <summary>Implicit cas operator to convert a memory attribute value to a raw integral value</summary>
        /// <param name="v"><see cref="MemoryAttributeValue"/> to convert</param>
        public static implicit operator UInt32(MemoryAttributeValue v) => v.ToUInt32();

        private readonly UInt32 Value = 0;

        // Gets a value (pre-shifted) and the mask for the bits in a field.
        private static (UInt32 Value, UInt32 Mask) GetValueAndMask(MemLocation loc, MemAccess value)
        {
            (int shiftVal, UInt32 mask) = GetShiftAndMask(loc);
            return ( ((UInt32)value << shiftVal) & mask, mask );
        }

        // Gets the shift value and mask (pre-shifted) for a field
        private static (int ShiftVal, UInt32 Mask) GetShiftAndMask(MemLocation loc)
        {
            int shiftVal = (BitsPerLocation * (int)loc);
            return (shiftVal, LocationMask << shiftVal);
        }

        private const int BitsPerLocation = 2;
        private const UInt32 LocationMask = (1u << BitsPerLocation) - 1u;
    }
}
