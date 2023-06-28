using System.Threading;
using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Bll.LoginBizRules;
using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Monitoring.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginBizRule _loginBizRule;

    public LoginController(ILogger<LoginController> logger, ILoginBizRule loginBizRule)
    {
        _logger = logger;
        _loginBizRule = loginBizRule;
    }

    [HttpPost]
    public async ValueTask<BaseResponse<Guid>> Login(LoginInfo login, CancellationToken cancellationToken)
    {
        var result = await _loginBizRule.Login(login, cancellationToken);
        return result.ToResponse();
    }
}
