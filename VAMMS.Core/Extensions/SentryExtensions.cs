using Microsoft.AspNetCore.Mvc;
using Sentry;
using VAMMS.Shared.Dtos;

namespace VAMMS.Core.Extensions;

public static class SentryExtensions
{
    public static ActionResult ReturnActionResult(this SentryId id)
    {
        return new BadRequestObjectResult(new ResponseDto<string>
        {
            StatusCode = 500,
            Message = "An error has occurred",
            Data = id.ToString()
        });
    }
}
