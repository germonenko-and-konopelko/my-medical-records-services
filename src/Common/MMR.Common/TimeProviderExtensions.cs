namespace MMR.Common;

public static class TimeProviderExtensions
{
    public static DateOnly GetUtcDateNow(this TimeProvider timeProvider)
    {
        return DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
    }
}