using AltV.Community.Events;
using CnR.Server.Features.Accounts.Abstractions;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Accounts.Events;

public sealed class AccountLoggedInEvent(IEventInvoker invoker)
    : Event<IAltCharacter>(invoker),
        IAccountLoggedInEvent { }
