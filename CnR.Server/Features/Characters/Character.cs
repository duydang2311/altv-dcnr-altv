using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using CnR.Server.Domain.Models;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Characters;

public sealed class Character(ICore core, IntPtr nativePointer, uint id)
    : AsyncPlayer(core, nativePointer, id),
        ICharacter
{
    CharacterId? ICharacter.Id { get; set; }
    public AccountId AccountId { get; set; }
}
