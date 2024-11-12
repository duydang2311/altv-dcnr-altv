using AltV.Net.Data;
using AltV.Net.Enums;
using CnR.Server.Common;
using CnR.Server.Features.Accounts.Abstractions;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Characters.Scripts;

public sealed class SpawnCharacterScript(IAccountLoggedInEvent accountLoggedInEvent) : Script
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        accountLoggedInEvent.AddHandler(OnAccountLoggedIn);
        return Task.CompletedTask;
    }

    private void OnAccountLoggedIn(ICharacter character)
    {
        character.Model = (uint)PedModel.FreemodeMale01;
        character.Spawn(new Position(-31.010f, 6316.830f, 40.083f));
    }
}
