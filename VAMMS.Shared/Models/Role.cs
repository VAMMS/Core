using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace VAMMS.Shared.Models;

[Index(nameof(Name))]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<User>? Users { get; set; }
}
