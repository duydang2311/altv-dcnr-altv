using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiFactory : IUiFactory
{
    public IWebView Create(ICore core, nint entityPointer, uint id)
    {
        return new Ui(core, entityPointer, id);
    }
}
