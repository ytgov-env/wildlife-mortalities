namespace WildlifeMortalities.App.HostedServices;

public abstract class TimerBasedHostedService : IHostedService
{
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _period;
    private Timer? _timer;
    private bool _isRunning;

    protected TimerBasedHostedService(TimeSpan dueTime, TimeSpan period)
    {
        _dueTime = dueTime;
        _period = period;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWorkInternal, null, _dueTime, _period);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void DoWorkInternal(object? state)
    {
        if (_isRunning)
        {
            return;
        }

        try
        {
            _isRunning = true;
            await DoWork(state);
        }
        finally
        {
            _isRunning = false;
        }
    }

    protected abstract Task DoWork(object? state);
}
