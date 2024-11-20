namespace CnR.Shared.Uis;

public sealed class Route
{
    public string Value { get; }

    private Route(string value)
    {
        Value = value;
    }

    public static readonly Route SignIn = new("sign_in");
    public static readonly Route GamemodeSelection = new("gamemode_selection");
}
