using Microsoft.Extensions.Logging;

namespace MMR.Common;

public static class LogEvents
{
    public static readonly EventId UnhandledException = new(100, nameof(UnhandledException));
}