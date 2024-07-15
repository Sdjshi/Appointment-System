using Microsoft.AspNetCore.Mvc;
using AppointmentData.DataAccessLayer;
using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppointmentController : Controller
    {
        private readonly AppointmentDAL _appointmentDAL;
        private readonly IServiceDAL _serviceDAL;

        public AppointmentController(AppointmentDAL appointmentDAL,IServiceDAL serviceDAL)
        {
            _appointmentDAL = appointmentDAL;
            _serviceDAL = serviceDAL;
        }

        public IActionResult Index()
        {
            IEnumerable<Service> service = _serviceDAL.GetAllService();
            return View(service);
        }


        


        public IActionResult BookAppointment(int ServiceId, int ServiceProviderId)
        {
            List<SlotModel> slotModels = _appointmentDAL.GetSlotsByServiceAndServiceProvider(ServiceId, ServiceProviderId);
            return View(slotModels);
        }


        public ActionResult ConfirmAppointment(string date, string time, int serviceId, int serviceProviderId)
        {
            DateTime appointmentDateTime = DateTime.Parse(date + " " + time);

           
            ViewBag.AppointmentDate = date;
            ViewBag.AppointmentTime = time;

            ViewBag.ServiceId = serviceId;
            ViewBag.ServiceProviderId = serviceProviderId;

            return View();
        }


        [HttpPost]
        public IActionResult BookAppointmentComplete(string date, string time, string name, string address, string phone, string email , int serviceId ,int serviceProviderId)
        {
            _appointmentDAL.BookAppointmentComplete(date, time, name, address, phone, email,serviceId,serviceProviderId);
            return Json(new { success = true, message = "Appointment booked successfully!" });
        }

        public IActionResult GetAppointments(DateTime date, int serviceId, int serviceProviderId)
        {
            try
            {
                var appointmentTimes = _appointmentDAL.GetAppointments(date, serviceId, serviceProviderId);

                return Ok(new { appointmentTimes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [AllowAnonymous]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAllAppointments()
        {
            var AppontmentList = _appointmentDAL.GetAllAppointment();
            return View(AppontmentList);
        }

        [HttpPost]
        public ActionResult StatusCancelAppointment(Appointment appointment)
        {
            try
            {
                _appointmentDAL.StatusCancelAppointment(appointment);
                return Json(appointment);
            }
            catch
            {
                return Json(appointment);
            }
        }
        public IActionResult UpdateAppointmentStatus()
        {
            
            _appointmentDAL.UpdateAppointmentStatus();
            return Ok();
        }

        [AllowAnonymous]
        [Authorize(Roles ="User")]

        public IActionResult GetMyAppointments(int userId)
        {

            var AppontmentList = _appointmentDAL.GetMyAppointments(userId);
            
            return View(AppontmentList);
        }
    }
}
