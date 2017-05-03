using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SecurityApp.Security
{
    public class InvoiceAuthorizationRequirementHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        private readonly IAuthorizationService _authorizationService;

        public InvoiceAuthorizationRequirementHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Invoice invoice)
        {
            if (await _authorizationService.AuthorizeAsync(context.User, "InvoiceReader"))
            {
                if (invoice.MandantId == 2)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}