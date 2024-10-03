using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using CnR.Server.Domain.Models;
using CnR.Server.Players.Abstractions;

namespace CnR.Server.Characters;

public sealed class Character(ICore core, IntPtr nativePointer, uint id)
    : AsyncPlayer(core, nativePointer, id),
        ICharacter
{
    CharacterId? ICharacter.Id { get; set; }
}
