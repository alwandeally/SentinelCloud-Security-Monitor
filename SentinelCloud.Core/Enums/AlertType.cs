namespace SentinelCloud.Core.Enums;

public enum AlertType
{
    RepeatedFailedLogins = 1,
    FailedThenSuccess = 2,
    UnusualLoginHour = 3,
    MultipleUsernamesFromSameIp = 4
}
