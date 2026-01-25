using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace DevopsIntelli.API.Filter;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {

        // for prod:
        // var httpContext = context.GetHttpContext();
        // return httpContext.User.Identity?.IsAuthenticated == true &&
        //        httpContext.User.IsInRole("Admin");
        return true;
    }
}
