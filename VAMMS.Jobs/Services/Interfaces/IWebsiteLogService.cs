namespace VAMMS.Jobs.Services.Interfaces;

public interface IWebsiteLogService
{
    /// <summary>
    /// Create a website log
    /// </summary>
    /// <param name="action">Action being performed</param>
    /// <param name="oldData">Old data as json</param>
    /// <param name="newData">New data as json</param>
    Task CreateWebsiteLog(string action, string oldData, string newData);
}