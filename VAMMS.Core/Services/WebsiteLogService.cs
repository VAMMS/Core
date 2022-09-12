using VAMMS.Core.Data;
using VAMMS.Core.Services.Interfaces;
using VAMMS.Shared.Models;

namespace VAMMS.Core.Services;

public class WebsiteLogService : IWebsiteLogService
{
    private DatabaseContext _context;

    public WebsiteLogService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateWebsiteLog(HttpRequest request, string action, string oldData, string newData)
    {
        var ip = request.Headers["CF-Connecting-IP"].ToString() ??
                 request.HttpContext.Connection?.RemoteIpAddress?.ToString() ?? "Not Found";
        await _context.WebsiteLogs.AddAsync(new WebsiteLog
        {
            IpAddress = ip == "::1" ? "localhost" : ip,
            Action = action,
            OldData = oldData,
            NewData = newData
        });
        await _context.SaveChangesAsync();
    }
}
