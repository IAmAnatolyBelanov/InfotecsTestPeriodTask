using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Infotecs.Monitoring.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Infotecs.Monitoring.Bll.LoginBizRules;
public class LoginBizRule : ILoginBizRule, IDisposable, IAsyncDisposable
{
    private readonly MonitoringContext _context;
    private readonly ILogger<LoginBizRule> _logger;
    private readonly IClock _clock;

    public LoginBizRule(MonitoringContext context, ILogger<LoginBizRule> logger, IClock clock)
    {
        _context = context;
        _logger = logger;
        _clock = clock;
    }

    public async ValueTask<Guid> Login(LoginInfo login, CancellationToken cancellationToken)
    {
        login.Id = Guid.NewGuid();
        login.DateTime = _clock.UtcNow;

        await _context.Logins.AddAsync(login, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return login.Id;
    }

    public void Dispose() => _context.Dispose();
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
