using VAMMS.Jobs.Data;
using VAMMS.Jobs.Services.Interfaces;
using VAMMS.Shared.Models;

namespace VAMMS.Jobs.Services;

public class WebsiteLogService : IWebsiteLogService
{
    private DatabaseContext _context;

    public WebsiteLogService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateWebsiteLog(string action, string oldData, string newData)
    {
        await _context.WebsiteLogs.AddAsync(new WebsiteLog
        {
            IpAddress = "SYSTEM",
            Action = action,
            OldData = oldData,
            NewData = newData
        });
        await _context.SaveChangesAsync();
    }
}
