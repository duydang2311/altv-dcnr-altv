namespace CnR.Shared;

public static class EnumerableHelper
{
    public static IEnumerable<T> Combine<T>(params T[] elements)
    {
        return elements;
    }

    public static Action Merge(params Action[] actions)
    {
        return () =>
        {
            foreach (var a in actions)
            {
                a();
            }
        };
    }
}
