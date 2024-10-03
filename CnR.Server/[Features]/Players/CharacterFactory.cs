using AltV.Net;
using AltV.Net.Elements.Entities;
using CnR.Server.Players.Abstractions;

namespace CnR.Server.Characters;

public sealed class CharacterFactory : ICharacterFactory
{
    public IPlayer Create(ICore core, nint entityPointer, uint id)
    {
        return new Character(core, entityPointer, id);
    }
}
