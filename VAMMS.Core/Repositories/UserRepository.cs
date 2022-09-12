using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VAMMS.Core.Data;
using VAMMS.Core.Repositories.Interfaces;
using VAMMS.Core.Services.Interfaces;
using VAMMS.Core.Utils;
using VAMMS.Shared.Dtos;
using VAMMS.Shared.Enums;
using VAMMS.Shared.Models;

namespace VAMMS.Core.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly IVatsimService _vatsimService;
    private readonly IWebsiteLogService _websiteLogService;
    private readonly IMapper _mapper;

    public UserRepository(DatabaseContext context, IVatsimService vatsimService, IWebsiteLogService websiteLogService, IMapper mapper)
    {
        _context = context;
        _vatsimService = vatsimService;
        _websiteLogService = websiteLogService;
        _mapper = mapper;
    }

    public async Task<VisitorApplication> CreateVisitorApplication(VisitorApplicationDto dto, HttpRequest request)
    {
        var result = await _context.VisitorApplications.AddAsync(new VisitorApplication
        {
            Cid = dto.Cid,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Rating = dto.Rating,
            RatingHours = await _vatsimService.GetRatingHours(dto.Cid, dto.Rating),
            VisitorFrom = dto.VisitorFrom,
            Reason = dto.Reason
        });
        await _context.SaveChangesAsync();

        var newData = JsonConvert.SerializeObject(result.Entity);
        await _websiteLogService.CreateWebsiteLog(request, "Created visitor application", string.Empty, newData);

        return result.Entity;
    }

    public async Task<IList<UserDto>> GetUsers()
    {
        var users = await _context.Users
            .Where(x => x.Status != UserStatus.Removed)
            .OrderBy(x => x.LastName)
            .ToListAsync();
        return _mapper.Map<IList<User>, IList<UserDto>>(users);
    }

    public async Task<UserDto> GetUser(int userId)
    {
        var user = await _context.Users.Where(x => x.Status != UserStatus.Removed).FirstOrDefaultAsync() ??
            throw new UserNotFoundException($"User '{userId}' not found");
        return _mapper.Map<User, UserDto>(user);
    }
}
