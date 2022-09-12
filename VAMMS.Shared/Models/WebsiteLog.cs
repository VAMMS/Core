using Microsoft.EntityFrameworkCore;

namespace VAMMS.Shared.Models;

[Index(nameof(IpAddress))]
[Index(nameof(Action))]
[Index(nameof(Timestamp))]
public class WebsiteLog
{
    public int Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string OldData { get; set; } = string.Empty;
    public string NewData { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
