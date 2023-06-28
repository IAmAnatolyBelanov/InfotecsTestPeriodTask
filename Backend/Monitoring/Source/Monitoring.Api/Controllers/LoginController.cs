using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Dal;

namespace Infotecs.Monitoring.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly MonitoringContext _context;

    public LoginController(ILogger<LoginController> logger, MonitoringContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async ValueTask<Guid> Login(LoginInfo login, CancellationToken cancellationToken)
    {
        if (login.Id != default)
            throw new Exception("Не клиент выдаёт айдишник!");

        login.DateTime = DateTime.UtcNow;

        await _context.Logins.AddAsync(login);
        await _context.SaveChangesAsync();

        return login.Id;
    }
}
