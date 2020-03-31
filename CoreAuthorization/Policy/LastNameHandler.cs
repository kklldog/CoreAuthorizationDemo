using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAuthorization.Policy
{
    public class LastNamRequirement : IAuthorizationRequirement
    {
        public string LastName { get; set; }
    }

    public class LastNameHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            var lastNameRequirement = requirement as LastNamRequirement;
            if (lastNameRequirement == null)
            {
                return Task.CompletedTask;
            }

            var isTeacher = context.User.HasClaim((c) =>
            {
                return c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "老师";
            });
            var isWang = context.User.HasClaim((c) =>
            {
                return c.Type == "LastName" && c.Value == lastNameRequirement.LastName;
            });

            if (isTeacher && isWang)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
