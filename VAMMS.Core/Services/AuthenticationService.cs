using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VAMMS.Core.Data;
using VAMMS.Shared.Enums;
using VATSIM.Connect.AspNetCore.Server.Services;
using VATSIM.Connect.AspNetCore.Shared.DTO;

namespace VAMMS.Core.Services;

public class AuthenticationService : IVatsimAuthenticationService
{

    private readonly DatabaseContext _context;

    public AuthenticationService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Claim>> ProcessVatsimUserLogin(VatsimUserDto user)
    {
        var claims = new List<Claim>();
        var u = await _context.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == user.Cid);
        if (u == null || u.Status == UserStatus.Removed)
        {
            claims.Add(new Claim("Rating", $"{user.VatsimDetails.ControllerRating}"));
            claims.Add(new Claim("IsMember", $"{false}"));
            return claims;
        }

        claims.Add(new Claim("IsMember", $"{true}"));
        claims.Add(new Claim("Rating", $"{u.Rating}"));

        if (u.Roles == null) return claims;
        claims.AddRange(u.Roles
            .Select(_ =>
                new Claim(ClaimTypes.Role, _.Name)
            ));

        return claims;
    }
}
