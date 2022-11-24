namespace WildlifeMortalities.App.HostedServices;

public abstract class TimerBasedHostedService : IHostedService
{
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _period;
    private Timer? _timer;

    protected TimerBasedHostedService(TimeSpan dueTime, TimeSpan period)
    {
        _dueTime = dueTime;
        _period = period;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, _dueTime, _period);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    protected abstract void DoWork(object? state);
}
