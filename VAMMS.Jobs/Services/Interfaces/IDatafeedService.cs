using VAMMS.Jobs.Dtos;

namespace VAMMS.Jobs.Services.Interfaces;

public interface IDatafeedService
{
    /// <summary>
    /// Get the datafeed url from the status page
    /// </summary>
    /// <returns>Datafeed url</returns>
    Task<string> GetDatafeedUrl();

    /// <summary>
    /// Get datafeed given the url found
    /// </summary>
    /// <returns>Datafeed</returns>
    Task<Datafeed?> GetDatafeed(string url);
}
