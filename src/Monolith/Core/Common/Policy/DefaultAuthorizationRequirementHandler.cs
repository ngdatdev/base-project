using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Common.Policy;

/// <summary>
/// The default authorization requirement handler
/// </summary>
public class DefaultAuthorizationRequirementHandler
    : AuthorizationHandler<DefaultAuthorizationRequirement>
{
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public DefaultAuthorizationRequirementHandler(Lazy<IHttpContextAccessor> httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DefaultAuthorizationRequirement requirement
    )
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail();

            return Task.CompletedTask;
        }

        var purposeClaimValue = context.User.FindFirstValue(Constant.ClaimType.PURPOSE.Name);
        if (!Equals(purposeClaimValue, Constant.ClaimType.PURPOSE.Value.USER_IN_APP))
        {
            context.Fail();

            return Task.CompletedTask;
        }

        var httpContext = _httpContextAccessor.Value.HttpContext;
        httpContext.Items.Add(
            Constant.ClaimType.SUB,
            context.User.FindFirstValue(Constant.ClaimType.SUB)
        );

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
