using Infotecs.Monitoring.Dal.Models;

namespace Infotecs.Monitoring.Bll.LoginBizRules;
public interface ILoginBizRule
{
    ValueTask<Guid> Login(LoginInfo login, CancellationToken cancellationToken);
}
