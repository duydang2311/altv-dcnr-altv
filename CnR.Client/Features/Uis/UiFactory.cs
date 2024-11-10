using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiFactory(IUiMessagingContextFactory uiMessagingContextFactory) : IUiFactory
{
    public IWebView Create(ICore core, nint baseObjectPointer, uint id)
    {
        return new Ui(core, baseObjectPointer, id, uiMessagingContextFactory);
    }
}
