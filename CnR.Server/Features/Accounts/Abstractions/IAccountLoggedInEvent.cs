using AltV.Community.Events;
using CnR.Server.Players.Abstractions;

namespace CnR.Server.Features.Accounts.Abstractions;

public interface IAccountLoggedInEvent : IEvent<IAltCharacter> { }
