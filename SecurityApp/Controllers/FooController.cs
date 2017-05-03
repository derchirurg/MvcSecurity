using Microsoft.AspNetCore.Authorization.Infrastructure;
using SecurityApp.Security;

namespace SecurityApp.Controllers
{
    public interface IInvoiceService
    {
        Invoice FindInvoice(int id);
    }

    public class InvoiceService : IInvoiceService
    {
        public Invoice FindInvoice(int id)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class Operations
    {
        public static readonly OperationAuthorizationRequirement Read
            = new OperationAuthorizationRequirement() { Name = "read" };

        public static readonly OperationAuthorizationRequirement Write
            = new OperationAuthorizationRequirement() { Name = "write" };
    }
}