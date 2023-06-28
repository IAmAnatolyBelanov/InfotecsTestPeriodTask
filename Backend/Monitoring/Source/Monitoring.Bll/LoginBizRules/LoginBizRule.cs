using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Microsoft.Extensions.Logging;

namespace Infotecs.Monitoring.Bll.LoginBizRules;
public class LoginBizRule : ILoginBizRule, IDisposable, IAsyncDisposable
{
    private readonly MonitoringContext _context;
    private readonly IClock _clock;

    public LoginBizRule(MonitoringContext context, IClock clock)
    {
        _context = context;
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
