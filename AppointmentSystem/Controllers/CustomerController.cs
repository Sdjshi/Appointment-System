using AppointmentData.DataAccessLayer;
using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerDAL _customerDAL;

        public CustomerController(ICustomerDAL customerDAL)
        {
            _customerDAL = customerDAL;
        }
        public ActionResult GetAllCustomer()
        {
            IEnumerable<Appointment> customer = _customerDAL.GetAllCustomer();
            return View(customer);
        }
        [HttpPost]

        public ActionResult CreateCustomer(Customer Customer)
        {

            try
            {
                _customerDAL.AddCustomer(Customer);
                return RedirectToAction(nameof(GetAllCustomer));
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]

        public ActionResult EditCustomer(Customer Customer)
        {

            try
            {
                _customerDAL.EditCustomer(Customer);
                return RedirectToAction(nameof(GetAllCustomer));
            }
            catch
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult StatusDeleteCustomer(Customer Customer)
        {
            try
            {
                _customerDAL.StatusDeleteCustomer(Customer.CustomerId);
                return Json(Customer);
            }
            catch
            {
                return Json(Customer);
            }
        }
        [HttpPost]
        public ActionResult GetDetailsById(int CustomerID)
        {
            Customer Customer = _customerDAL.GetCustomerData(CustomerID);
            return Json(Customer);
        }

        public IActionResult GetCustomerByPhone(string phoneNumber)
        {
         
            var customer = _customerDAL.GetCustomerByPhone(phoneNumber);

            if (customer != null)
            {
               
                return Json(new
                {
                    name = customer.CustomerFullName,
                    address = customer.CustomerAddress,
                    email = customer.CustomerEmail
                });
            }
            else
            {
               
                return NotFound();
            }
        }
    }
}
