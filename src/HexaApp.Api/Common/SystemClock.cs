using HexaApp.SharedKernel;

namespace HexaApp.Api.Common;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
