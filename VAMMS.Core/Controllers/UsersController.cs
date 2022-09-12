using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using VAMMS.Core.Extensions;
using VAMMS.Core.Repositories.Interfaces;
using VAMMS.Core.Utils;
using VAMMS.Shared.Dtos;

namespace VAMMS.Core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IHub _sentryHub;
    private readonly IValidator<VisitorApplicationDto> _validator;

    public UsersController(IUserRepository userRepository, IHub sentryHub, IValidator<VisitorApplicationDto> validator)
    {
        _userRepository = userRepository;
        _sentryHub = sentryHub;
        _validator = validator;
    }

    [HttpPost("visit")]
    //[Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ResponseDto<IList<ValidationFailure>>), 400)]
    [ProducesResponseType(typeof(ResponseDto<Guid>), 500)]
    public async Task<ActionResult> CreateVisitorApplication(VisitorApplicationDto dto)
    {
        try
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return BadRequest(new ResponseDto<IList<ValidationFailure>>
                {
                    StatusCode = 400,
                    Message = "Validation error",
                    Data = result.Errors
                });
            }
            await _userRepository.CreateVisitorApplication(dto, Request);
            return Ok(new ResponseDto<string>
            {
                StatusCode = 200,
                Message = "Created visitor application",
                Data = string.Empty
            });
        }
        catch (VatsimRatingTimesNotFoundException ex)
        {
            return StatusCode(500, new ResponseDto<string>
            {
                StatusCode = 500,
                Message = ex.Message,
                Data = string.Empty
            });
        }
        catch (Exception ex)
        {
            return _sentryHub.CaptureException(ex).ReturnActionResult();
        }
    }

    [HttpGet]
    //[Authorize]
    [ProducesResponseType(typeof(ResponseDto<IList<UserDto>>), 200)]
    [ProducesResponseType(typeof(ResponseDto<Guid>), 500)]
    public async Task<ActionResult<IList<UserDto>>> GetUsers()
    {
        try
        {
            var result = await _userRepository.GetUsers();
            return Ok(new ResponseDto<IList<UserDto>>
            {
                StatusCode = 200,
                Message = $"Got {result.Count} users",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return _sentryHub.CaptureException(ex).ReturnActionResult();
        }
    }

    [HttpGet("{userId:int}")]
    //[Authorize]
    [ProducesResponseType(typeof(ResponseDto<UserDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<string>), 400)]
    [ProducesResponseType(typeof(ResponseDto<Guid>), 500)]
    public async Task<ActionResult<IList<UserDto>>> GetUser(int userId)
    {
        try
        {
            var result = await _userRepository.GetUser(userId);
            return Ok(new ResponseDto<UserDto>
            {
                StatusCode = 200,
                Message = $"Got user '{userId}'",
                Data = result
            });
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResponseDto<string>
            {
                StatusCode = 500,
                Message = ex.Message,
                Data = string.Empty
            });
        }
        catch (Exception ex)
        {
            return _sentryHub.CaptureException(ex).ReturnActionResult();
        }
    }
}
