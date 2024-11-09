using AltV.Net;
using AltV.Net.Elements.Entities;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Characters;

public sealed class AltCharacterFactory : IAltCharacterFactory
{
    public IPlayer Create(ICore core, nint entityPointer, uint id)
    {
        return new AltCharacter(core, entityPointer, id);
    }
}
