using System.Diagnostics.CodeAnalysis;

namespace CnR.Shared.Effects;

public readonly struct GenericError
{
    public readonly object value;

    private GenericError(object value)
    {
        this.value = value;
    }

    public bool TryCatch<T>([NotNullWhen(true)] out T? value)
    {
        if (this.value is T valueT)
        {
            value = valueT;
            return true;
        }
        value = default;
        return false;
    }

    public static GenericError From(object value) => new GenericError(value);

    bool Equals(GenericError other) => value == other.value;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is GenericError o && Equals(o);
    }

    public override string ToString() => value.ToString() ?? string.Empty;

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
}

