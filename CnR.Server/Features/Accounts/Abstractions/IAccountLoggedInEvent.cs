using AltV.Community.Events;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Accounts.Abstractions;

public interface IAccountLoggedInEvent : IEvent<IAltCharacter> { }
