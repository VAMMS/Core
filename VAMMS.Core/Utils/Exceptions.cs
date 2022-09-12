namespace VAMMS.Core.Utils;

public class VatsimRatingTimesNotFoundException : Exception
{
    public VatsimRatingTimesNotFoundException(string message) : base(message)
    {
    }
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message)
    {
    }
}
