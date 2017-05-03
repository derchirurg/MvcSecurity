using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace SecurityApp.Controllers
{
    [Authorize("Is16")]
    public class DrinkController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IInvoiceService _invoiceService;

        public DrinkController(
            IDataProtectionProvider protectionProvider,
            IAuthorizationService authorizationService,
            IInvoiceService invoiceService)
        {
            _authorizationService = authorizationService;
            _invoiceService = invoiceService;
        }


        [Authorize("InvoiceReader")]
        public async Task<IActionResult> Invoice(int id)
        {
            var invoice = _invoiceService.FindInvoice(id);
            if (invoice == null)
                return NotFound();


            if (await _authorizationService.AuthorizeAsync(User, "Fo"))
            {
                return View();
            }

            return NotFound();
        }
    }
}