using VAMMS.Shared.Enums;
using VAMMS.Shared.Models;

namespace VAMMS.Shared.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Rating Rating { get; set; }
    public DateTimeOffset Joined { get; set; }
    public AirportCertification Minor { get; set; }
    public AirportCertification Major { get; set; }
    public CenterCertification Center { get; set; }
    public bool Visitor { get; set; }
    public string? VisitorFrom { get; set; }
    public ICollection<Role>? Roles { get; set; }
    public UserStatus Status { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
}
