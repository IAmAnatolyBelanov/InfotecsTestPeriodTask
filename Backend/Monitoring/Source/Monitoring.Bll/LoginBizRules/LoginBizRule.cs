using Microsoft.Extensions.Logging;
using Monitoring.Dal;

namespace Infotecs.Monitoring.Bll.LoginBizRules;
public class LoginBizRule : ILoginBizRule, IDisposable, IAsyncDisposable
{
    private readonly MonitoringContext _context;
    private readonly ILogger<LoginBizRule> _logger;

    public LoginBizRule(MonitoringContext context, ILogger<LoginBizRule> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask<Guid> Login(LoginInfo login, CancellationToken cancellationToken)
    {
        if (login.Id != default)
            throw new Exception("Не клиент выдаёт айдишник!");

        login.DateTime = DateTime.UtcNow;

        await _context.Logins.AddAsync(login, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return login.Id;
    }

    public void Dispose() => _context.Dispose();
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
