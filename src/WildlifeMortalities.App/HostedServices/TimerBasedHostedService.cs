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

    protected TimerBasedHostedService(TimeOnly startTime, TimeSpan period)
    {
        var timeNow = TimeOnly.FromDateTime(DateTime.Now);
        if (startTime >= timeNow)
        {
            _dueTime = startTime - TimeOnly.FromDateTime(DateTime.Now);
        }
        else
        {
            _dueTime = DateTime.Now.AddDays(1).Date.Add(startTime.ToTimeSpan()) - DateTime.Now;
        }

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
        catch (Exception e)
        {
            Log.Error("Exception {e} in hosted service", e);
        }
        finally
        {
            _isRunning = false;
        }
    }

    protected abstract Task DoWork(object? state);
}
