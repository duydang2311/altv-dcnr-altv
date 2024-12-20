namespace CnR.Shared.Effects;

public static class EffectHelper
{
    public static Func<Task<Effect<TReturn, GenericError>>> Try<TReturn>(Func<Task<TReturn>> f)
    {
        return async () =>
        {
            try
            {
                return Effect.Succeed(await f().ConfigureAwait(false));
            }
            catch (Exception e)
            {
                return Effect.Fail(GenericError.From(e));
            }
        };
    }
}
