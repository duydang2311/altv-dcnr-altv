using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class Ui(ICore core, IntPtr webViewNativePointer, uint id)
    : WebView(core, webViewNativePointer, id),
        IUi { }
