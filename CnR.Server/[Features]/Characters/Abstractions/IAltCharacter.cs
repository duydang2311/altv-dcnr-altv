using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using CnR.Server.Domain.Models;

namespace CnR.Server.Players.Abstractions;

public interface IAltCharacter : IPlayer, IAsyncConvertible<IPlayer>
{
    new CharacterId? Id { get; set; }
}
