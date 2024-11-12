using AltV.Net.Elements.Entities;
using CnR.Shared.Uis;
using OneOf.Types;

namespace CnR.Server.Features.Uis.Abstractions;

public interface IUi
{
    void Mount(IPlayer player, Route route, object? prop = null);
    Task<Effect<None, GenericError>> MountAsync(IPlayer player, Route route, object? prop = null);
    void Unmount(IPlayer player, Route route);
    Task<Effect<None, GenericError>> UnmountAsync(IPlayer player, Route route);
}
