using VAMMS.Shared.Enums;

namespace VAMMS.Core.Services.Interfaces;

public interface IVatsimService
{
    /// <summary>
    /// Get rating hours of a given user
    /// </summary>
    /// <param name="cid">Cid of user to get</param>
    /// <param name="rating">Rating to check</param>
    /// <returns>Double of hours in given rating</returns>
    /// <exception cref="VatsimRatingTimesNotFoundException">Times not found</exception>
    Task<double> GetRatingHours(int cid, Rating rating);
}