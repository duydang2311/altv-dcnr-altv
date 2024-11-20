using AltV.Net.Client;
using CnR.Client.Features.Games.Abstractions;

namespace CnR.Client.Features.Games;

public sealed class Game : IGame
{
    private int disabledControlsCounter;
    private int showCursorCounter;

    public void ToggleControls(bool toggle)
    {
        if (!toggle)
        {
            if (Interlocked.Increment(ref disabledControlsCounter) > 0 && Alt.GameControlsEnabled)
            {
                Alt.GameControlsEnabled = false;
            }
        }
        else if (Interlocked.Decrement(ref disabledControlsCounter) <= 0 && !Alt.GameControlsEnabled)
        {
            Alt.GameControlsEnabled = true;
        }
    }

    public void ToggleCursor(bool toggle)
    {
        if (toggle)
        {
            if (Interlocked.Increment(ref showCursorCounter) > 0 && !Alt.IsCursorVisible)
            {
                Alt.ShowCursor(true);
            }
        }
        else if (Interlocked.Decrement(ref showCursorCounter) <= 0 && Alt.IsCursorVisible)
        {
            Alt.ShowCursor(false);
        }
    }
}
