using CnR.Client.Common.Abstractions;

namespace CnR.Client.Common;

public abstract class Script : IHostedService
{
    public virtual Task StartAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
