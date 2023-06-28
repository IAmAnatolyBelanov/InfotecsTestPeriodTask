using Monitoring.Dal;

namespace Infotecs.Monitoring.Bll.LoginBizRules;
public interface ILoginBizRule
{
    ValueTask<Guid> Login(LoginInfo login, CancellationToken cancellationToken);
}
