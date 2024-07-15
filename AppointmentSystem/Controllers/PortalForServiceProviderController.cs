using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "User")]
    public class PortalForServiceProviderController : Controller
    {
        private readonly IServiceProviderDAL _serviceProviderDAL;

        public PortalForServiceProviderController(IServiceProviderDAL serviceProviderDAL)
        {
            _serviceProviderDAL = serviceProviderDAL;
        }
        public IActionResult Index()
        {
           return View();
        }

        public IActionResult TestIndex()
        {
            return View();
        }
       
    }
}
