using Microsoft.EntityFrameworkCore;
using VAMMS.Shared.Enums;

namespace VAMMS.Shared.Models;

[Index(nameof(Cid))]
[Index(nameof(FirstName))]
[Index(nameof(LastName))]
[Index(nameof(Email))]
[Index(nameof(Status))]
public class VisitorApplication
{
    public int Id { get; set; }
    public int Cid { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rating Rating { get; set; }
    public double RatingHours { get; set; }
    public string VisitorFrom { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public VisitorApplicationStatus Status { get; set; } = VisitorApplicationStatus.Pending;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
