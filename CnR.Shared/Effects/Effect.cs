using System.Diagnostics.CodeAnalysis;

namespace CnR.Shared.Effects;

public readonly struct Effect<TSuccess, TError>
{
    private readonly TSuccess success;
    private readonly TError error;
    private readonly bool isSuccess;

    public Effect(bool isSuccess, TSuccess success, TError error)
    {
        this.isSuccess = isSuccess;
        this.success = success;
        this.error = error;
    }

    public bool TryGetSuccess([NotNullWhen(true)] out TSuccess? success, [NotNullWhen(false)] out TError? error)
    {
        if (isSuccess)
        {
            success = this.success!;
            error = default;
            return true;
        }
        success = default;
        error = this.error!;
        return false;
    }

    public bool TryGetError([NotNullWhen(true)] out TError? error, [NotNullWhen(false)] out TSuccess? success)
    {
        if (!isSuccess)
        {
            error = this.error!;
            success = default;
            return true;
        }
        error = default;
        success = this.success!;
        return false;
    }

    public static implicit operator Effect<TSuccess, TError>(SuccessEffect<TSuccess> eff) => new Effect<TSuccess, TError>(true, eff.Value, default!);
    public static implicit operator Effect<TSuccess, TError>(ErrorEffect<TError> eff) => new Effect<TSuccess, TError>(false, default!, eff.Value);

    bool Equals(Effect<TSuccess, TError> other) =>
        isSuccess == other.isSuccess &&
        isSuccess switch
        {
            true => Equals(success, other.success),
            false => Equals(error, other.error),
        };

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is Effect<TSuccess, TError> o && Equals(o);
    }

    public override string ToString() =>
        isSuccess switch
        {
            true => success?.ToString(),
            false => error?.ToString(),
        } ?? string.Empty;

    public override int GetHashCode()
    {
        return isSuccess switch
        {
            true => HashCode.Combine(true, success),
            false => HashCode.Combine(false, error),
        };
    }
}

public readonly struct SuccessEffect<TSuccess>
{
    public readonly TSuccess Value { get; }

    public SuccessEffect(TSuccess value)
    {
        Value = value;
    }
}

public readonly struct ErrorEffect<TError>
{
    public readonly TError Value { get; }

    public ErrorEffect(TError value)
    {
        Value = value;
    }
}

public static class Effect
{
    public static SuccessEffect<TSuccess> Succeed<TSuccess>(TSuccess success)
    {
        return new SuccessEffect<TSuccess>(success);
    }

    public static ErrorEffect<TError> Fail<TError>(TError error)
    {
        return new ErrorEffect<TError>(error);
    }
}

