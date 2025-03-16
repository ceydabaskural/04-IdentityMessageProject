using IdentityMessageProject.EntityLayer.Concrete;
using IdentityMessageProject.PresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityMessageProject.PresentationLayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Model hatalıysa geri dön
            }

            // Kullanıcıyı e-posta ile bul
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu e-posta adresine sahip bir kullanıcı bulunamadı.");
                return View(model);
            }

            // E-posta doğrulaması yapılmamışsa
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Lütfen önce e-posta adresinizi doğrulayın.");
                return View(model);
            }

            // Giriş işlemini gerçekleştirelim
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);


            if (result.Succeeded)
            {
                return RedirectToAction("Inbox", "Message");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz giriş denemesi.");
                return View(model);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
