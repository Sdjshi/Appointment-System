using AppointmentData.DataAccessLayer;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AppointmentData.Interfaces;

namespace AppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountDAL _AccountDAL;
        private readonly IServiceProviderDAL _serviceProviderDAL;

        public AccountController(AccountDAL AccountDAL, IServiceProviderDAL serviceProviderDAL)
        {
            _AccountDAL = AccountDAL;
            _serviceProviderDAL = serviceProviderDAL;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _AccountDAL.AuthenticateUser(username, password);
            if (user != null)
            {
                var roles = _AccountDAL.GetRolesForUser(username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

                
                claims.Add(new Claim("UserId", user.UserId.ToString()));

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

              
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roles.Contains("User"))
                {
                    return RedirectToAction("Index", "PortalForServiceProvider");
                }
                else
                {
                  
                    return RedirectToAction("Index", "Default");
                }
            }

           
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

       

        [HttpPost]
        public IActionResult Create(AccountRegisterModel model)
        {
            var user = new AccountRegisterModel
            {
                Username = model.Username,
                Password = model.Password,
                RoleId = model.RoleId,
                ServiceProviderID = model.ServiceProviderID
            };

            try
            {
                if (_AccountDAL.CreateUser(user))
                {
                    return Json(new { success = true, message = "User created successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to create user account." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while creating the user account." });
            }
        }



        [HttpPost]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _AccountDAL.GetRoles();
            return Json(roles);
        }
        public ActionResult GetServiceProviders()
        {
            IEnumerable<ServiceProviders> serviceProviders = _serviceProviderDAL.GetAllServiceProviders();
            return Json(serviceProviders);
        }

        [HttpGet]
        public IActionResult GetServiceProviderEmail(int serviceProviderId)
        {
            ServiceProviders serviceProvider = _serviceProviderDAL.GetServiceProviderData(serviceProviderId);
            if (serviceProvider != null)
            {
                return Ok(serviceProvider.ServiceProviderEmail);
            }
            else
            {
                return NotFound(); 
            }
        }
    }

}
