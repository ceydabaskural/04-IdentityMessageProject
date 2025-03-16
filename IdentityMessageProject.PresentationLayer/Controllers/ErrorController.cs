using Microsoft.AspNetCore.Mvc;

namespace IdentityMessageProject.PresentationLayer.Controllers
{
    public class ErrorController : Controller
    {

        // 404 ve diğer HTTP hataları için yönlendirme Program.cs içinde
        //app.UseStatusCodePagesWithRedirects("/Error/Index");

        public IActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
