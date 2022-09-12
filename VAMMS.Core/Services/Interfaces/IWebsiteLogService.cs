namespace VAMMS.Core.Services.Interfaces;

public interface IWebsiteLogService
{
    /// <summary>
    /// Create a website log
    /// </summary>
    /// <param name="request">raw http request for ip</param>
    /// <param name="action">Action being performed</param>
    /// <param name="oldData">Old data as json</param>
    /// <param name="newData">New data as json</param>
    Task CreateWebsiteLog(HttpRequest request, string action, string oldData, string newData);
}