namespace System.WindowsInput.WinApi;

/// <summary>
/// Helper to convert enum to different underlying base type.
/// </summary>
/// <typeparam name="TEnum">Enum type.</typeparam>
/// <typeparam name="T">Underlying type.</typeparam>
public readonly struct EnumRebase<TEnum, T> where TEnum : Enum where T : unmanaged, IConvertible
{
    private readonly T value;

    /// <summary>
    /// Helper to convert enum to different underlying base type.
    /// </summary>
    /// <param name="val">Base value.</param>
    public EnumRebase(T val)
    {
        value = val;
    }

    /// <summary>
    /// Performs an implicit conversion from TEnum to T.
    /// </summary>
    public static implicit operator EnumRebase<TEnum, T>(TEnum enumVal)
    {
        return new EnumRebase<TEnum, T>((T)Convert.ChangeType(enumVal, typeof(T)));
    }

    /// <summary>
    /// Performs an implicit conversion from T to TEnum.
    /// </summary>
    public static implicit operator EnumRebase<TEnum, T>(T value)
    {
        return new EnumRebase<TEnum, T>(value);
    }

    /// <summary>
    /// Performs an implicit conversion from T to TEnum.
    /// </summary>
    public static implicit operator TEnum(EnumRebase<TEnum, T> er)
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), er.value);
    }
}
