using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VAMMS.Shared.Enums;

namespace VAMMS.Shared.Models;

[Index(nameof(FirstName))]
[Index(nameof(LastName))]
[Index(nameof(Email))]
[Index(nameof(Rating))]
[Index(nameof(Joined))]
[Index(nameof(Visitor))]
[Index(nameof(Status))]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rating Rating { get; set; }
    public DateTimeOffset Joined { get; set; }
    public AirportCertification Minor { get; set; } = AirportCertification.None;
    public AirportCertification Major { get; set; } = AirportCertification.None;
    public CenterCertification Center { get; set; } = CenterCertification.None;
    public bool Visitor { get; set; }
    public string? VisitorFrom { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Role>? Roles { get; set; }
    public UserStatus Status { get; set; }
    public bool ApiExempt { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
}
