using Microsoft.AspNetCore.Authorization;

namespace SecurityApp.Security
{
    public class InvoiceAuthorizationRequirement : IAuthorizationRequirement
    {
        public InvoiceAuthorizationRequirement(Action action)
        {
            Action = action;
        }

        public Action Action { get; }
    }
}