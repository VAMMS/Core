using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VATSIM.Connect.AspNetCore.Server.Authentication;
using VATSIM.Connect.AspNetCore.Server.Controllers;
using VATSIM.Connect.AspNetCore.Server.Options;
using VATSIM.Connect.AspNetCore.Server.Services;

namespace VAMMS.Core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController : AuthControllerBase
{
    public AuthController(IVatsimConnectService vatsimService, IJwtAuthManager jwtAuthManager,
        IVatsimAuthenticationService authenticationService, IOptions<VatsimServerOptions> vatsimServerOptions,
        ILogger<AuthController> logger) : base(vatsimService, jwtAuthManager, authenticationService,
        vatsimServerOptions, logger)
    {
    }
}