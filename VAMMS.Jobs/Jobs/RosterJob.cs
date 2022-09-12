using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using VAMMS.Jobs.Data;
using VAMMS.Jobs.Dtos;
using VAMMS.Jobs.Services.Interfaces;
using VAMMS.Shared.Enums;
using VAMMS.Shared.Models;

namespace VAMMS.Jobs.Jobs;

public class RosterJob : IJob
{

#nullable disable

    private ILogger<IJobsService> _logger;
    private DatabaseContext _context;
    private IVatusaService _vatusaService;
    private IWebsiteLogService _websiteLogService;

#nullable enable

    public async Task Execute(IJobExecutionContext context)
    {
        _logger = (ILogger<IJobsService>)context.Scheduler.Context.Get("Logger");
        _context = (DatabaseContext)context.Scheduler.Context.Get("DatabaseContext");
        _vatusaService = (IVatusaService)context.Scheduler.Context.Get("VatusaService");
        _websiteLogService = (IWebsiteLogService)context.Scheduler.Context.Get("WebsiteLogService");

        _logger.LogInformation("Running roster job...");
        var start = DateTime.UtcNow;

        try
        {
            var roster = await _vatusaService.GetRoster();

            if (roster == null)
            {
                _logger.LogError("Roster was null, stopping job");
                return;
            }

            await RemoveUsers(roster);
            await AddUsers(roster);
            await UpdateUsers(roster);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Roster job finished -- took {Time}s", Math.Round((DateTime.UtcNow - start).TotalSeconds, 3));
    }

    public async Task RemoveUsers(Roster roster)
    {
        var users = await _context.Users
            .Where(x => !x.ApiExempt)
            .Where(x => x.Status != UserStatus.Removed)
            .ToListAsync();
        foreach (var entry in users)
            if (!roster?.Data?.Any(x => x.Cid == entry.Id) ?? false)
            {
                var oldData = JsonConvert.SerializeObject(entry);
                entry.Status = UserStatus.Removed;
                await _websiteLogService.CreateWebsiteLog("Removed user from roster", oldData, JsonConvert.SerializeObject(entry));
            }
        await _context.SaveChangesAsync();
    }

    public async Task AddUsers(Roster roster)
    {
        if (roster.Data == null)
            return;
        foreach (var entry in roster.Data)
            if (!await _context.Users.AnyAsync(x => x.Id == entry.Cid))
            {
                var result = await _context.Users.AddAsync(new User
                {
                    Id = entry.Cid,
                    FirstName = entry.FirstName,
                    LastName = entry.LastName,
                    Initials = await GetInitials(entry.FirstName, entry.LastName),
                    Email = entry.Email,
                    Rating = entry.Rating,
                    Joined = entry.Joined,
                    Visitor = entry.Membership == "visit",
                    VisitorFrom = entry.Membership == "visit" ? entry.Facility : string.Empty,
                });
                await _websiteLogService.CreateWebsiteLog("Added user to roster", string.Empty, JsonConvert.SerializeObject(result.Entity));
            }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUsers(Roster roster)
    {
        if (roster.Data == null)
            return;
        foreach (var entry in roster.Data)
        {
            var changes = new List<string>();
            var user = await _context.Users.FindAsync(entry.Cid);
            if (user == null || user.ApiExempt || user.Status == UserStatus.Removed)
                continue;

            var oldData = JsonConvert.SerializeObject(user);

            if (user.FirstName != entry.FirstName)
            {
                user.FirstName = entry.FirstName;
                changes.Add("Updated First Name");
            }

            if (user.LastName != entry.LastName)
            {
                user.LastName = entry.LastName;
                changes.Add("Updated Last Name");
            }

            if (user.Email != entry.Email)
            {
                user.Email = entry.Email;
                changes.Add("Updated Email");
            }

            if (user.Rating != entry.Rating)
            {
                user.Rating = entry.Rating;
                changes.Add("Updated Rating");
            }

            if (!user.Visitor && (entry.Membership != "home"))
            {
                user.Visitor = true;
                user.VisitorFrom = entry.Facility;
                changes.Add("Updated Visitor Status");
            }

            if (user.Visitor && (entry.Membership == "home"))
            {
                user.Visitor = false;
                user.VisitorFrom = string.Empty;
                changes.Add("Updated Visitor Status");
            }

            if (changes.Count > 0)
            {
                user.Updated = DateTimeOffset.UtcNow;
                var newData = JsonConvert.SerializeObject(user);
                var actions = string.Join(",", changes);
                await _websiteLogService.CreateWebsiteLog(actions, oldData, newData);
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetInitials(string firstName, string lastName)
    {
        var initials = $"{firstName[0]}{lastName[0]}";
        var initialsExist = await _context.Users
            .Where(x => x.Initials.Equals(initials))
            .ToListAsync();
        if (!initialsExist.Any()) return initials;
        foreach (var letter in lastName)
        {
            initials = $"{firstName[0]}{letter.ToString().ToUpper()}";

            var exists = await _context.Users
                .Where(x => x.Initials.Equals(initials))
                .ToListAsync();

            if (!exists.Any()) return initials.ToUpper();
        }
        foreach (var letter in firstName)
        {
            initials = $"{letter.ToString().ToUpper()}{lastName[0]}";

            var exists = await _context.Users
                .Where(x => x.Initials.Equals(initials))
                .ToListAsync();

            if (!exists.Any()) return initials.ToUpper();
        }
        return string.Empty;
    }
}
