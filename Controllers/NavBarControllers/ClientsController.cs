using Microsoft.AspNetCore.Mvc;

namespace EdgeWEB.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
