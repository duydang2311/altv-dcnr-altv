using System.Diagnostics.CodeAnalysis;

namespace CnR.Shared.Effects;

public readonly struct Effect<TSuccess, TError>(bool isSuccess, TSuccess success, TError error)
{
    private readonly TSuccess success = success;
    private readonly TError error = error;
    private readonly bool isSuccess = isSuccess;

    public bool TryGetSuccess(
        [NotNullWhen(true)] out TSuccess? success,
        [NotNullWhen(false)] out TError? error
    )
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

    public bool TryGetError(
        [NotNullWhen(true)] out TError? error,
        [NotNullWhen(false)] out TSuccess? success
    )
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

    public static implicit operator Effect<TSuccess, TError>(SuccessEffect<TSuccess> eff) =>
        new(true, eff.Value, default!);

    public static implicit operator Effect<TSuccess, TError>(ErrorEffect<TError> eff) =>
        new(false, default!, eff.Value);

    bool Equals(Effect<TSuccess, TError> other) =>
        isSuccess == other.isSuccess
        && isSuccess switch
        {
            true => Equals(success, other.success),
            false => Equals(error, other.error),
        };

    public override bool Equals(object? obj)
    {
        if (obj is null)
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

    public static bool operator ==(Effect<TSuccess, TError> left, Effect<TSuccess, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Effect<TSuccess, TError> left, Effect<TSuccess, TError> right)
    {
        return !(left == right);
    }
}

public readonly struct SuccessEffect<TSuccess>(TSuccess value)
{
    public readonly TSuccess Value { get; } = value;
}

public readonly struct ErrorEffect<TError>(TError value)
{
    public readonly TError Value { get; } = value;
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
