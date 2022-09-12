using VAMMS.Shared.Dtos;
using VAMMS.Shared.Models;

namespace VAMMS.Core.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Create a visitor application
    /// </summary>
    /// <param name="dto">Dto object with visitor application data</param>
    /// <param name="request">Raw http request for logging</param>
    /// <returns>Created visitor application for use if needed</returns>
    Task<VisitorApplication> CreateVisitorApplication(VisitorApplicationDto dto, HttpRequest request);

    /// <summary>
    /// Get all users who are not removed
    /// </summary>
    /// <returns>All users who are not removed</returns>
    Task<UserDto> GetUser(int userId);

    /// <summary>
    /// Get a user by id
    /// </summary>
    /// <param name="userId">Id of user to get</param>
    /// <returns>User if found</returns>
    /// <exception cref="UserNotFoundException">Thrown if user is not found</exception>
    Task<IList<UserDto>> GetUsers();
}