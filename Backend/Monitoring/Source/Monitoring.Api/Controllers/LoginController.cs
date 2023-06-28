using System.Threading;
using Infotecs.Monitoring.Bll.LoginBizRules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Api;
using Monitoring.Dal;

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
