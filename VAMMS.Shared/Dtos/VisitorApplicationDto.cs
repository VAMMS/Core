using VAMMS.Shared.Enums;

namespace VAMMS.Shared.Dtos;

public class VisitorApplicationDto
{
    public int Cid { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rating Rating { get; set; }
    public string VisitorFrom { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}
