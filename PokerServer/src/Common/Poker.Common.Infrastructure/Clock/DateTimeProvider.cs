namespace Poker.Common.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime GetUtcNow(int seconds)
    {
        return DateTime.UtcNow.AddSeconds(seconds);
    }
}