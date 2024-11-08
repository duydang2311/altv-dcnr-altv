namespace CnR.Client.Common.Abstractions;

public interface IHostedService
{
    Task StartAsync(CancellationToken ct);
    Task StopAsync(CancellationToken ct);
}
