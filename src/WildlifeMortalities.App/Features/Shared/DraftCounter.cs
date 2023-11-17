namespace WildlifeMortalities.App.Features.Shared;

public class DraftCounter
{
    public int Count { get; private set; }

    public event EventHandler CountChanged = null!;

    public void SetCounter(int value)
    {
        if (value == Count)
            return;

        Count = value;
        CountChanged?.Invoke(this, EventArgs.Empty);
    }
}
