using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Model.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTrackSystem.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;
        private readonly IUserService _userService;

        public AuthController(IAuthService auth, IUserService userService)
        {
            _auth = auth;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
                return View(registerRequest);

            var response = await _auth.RegisterAsync(registerRequest);

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(registerRequest);
            }

            TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _auth.LoginAsync(model);

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(model);
            }

            var token = response.Data.Token;
            var expires = response.Data.Expires;

            Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires
            });

            return RedirectToAction("Index", "Home");
        }
    }
}
