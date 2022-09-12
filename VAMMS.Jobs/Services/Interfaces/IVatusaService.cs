using VAMMS.Jobs.Dtos;

namespace VAMMS.Jobs.Services.Interfaces;

public interface IVatusaService
{
    /// <summary>
    /// Get the facility roster
    /// </summary>
    /// <returns>Roster data</returns>
    Task<Roster?> GetRoster();
}
