using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
using System.Linq;

namespace WebProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataBaseContext _context;

        public LoginController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Registration page
        public IActionResult Register()
        {
            return View();
        }
		[HttpPost]
		public IActionResult Index(string Email, string Password)
		{
			// Önce çalışan tablosunda kontrol edelim
			var employee = _context.Employees.FirstOrDefault(u => u.Email == Email && u.Password == Password);
			if (employee != null)
			{
				// Çalışan bilgilerini session'a kaydet
				HttpContext.Session.SetString("UserRole", employee.AuthLevel.ToString());
				HttpContext.Session.SetString("UserEmail", employee.Email);
				HttpContext.Session.SetInt32("UserId", employee.EmployeeId);

				// Servis ID kontrolü
				var serviceId = HttpContext.Session.GetString("ServiceId");
				if (int.TryParse(serviceId, out int parsedServiceId) && parsedServiceId > 0)
				{
					return RedirectToAction("Create", "Appointment", new { ServiceId = parsedServiceId });
				}
				return RedirectToAction("Index", "Home");
			}

			// Eğer çalışan değilse, müşteri olarak kontrol et
			var customer = _context.Customers.FirstOrDefault(u => u.Email == Email && u.Password == Password);
			if (customer != null)
			{
				// Müşteri bilgilerini session'a kaydet
				HttpContext.Session.SetString("UserRole", "Customer");
				HttpContext.Session.SetString("UserEmail", customer.Email);
				HttpContext.Session.SetInt32("UserId", customer.CustomerId);

				// Servis ID kontrolü
				var serviceId = HttpContext.Session.GetString("ServiceId");
				if (int.TryParse(serviceId, out int parsedServiceId) && parsedServiceId > 0)
				{
					return RedirectToAction("Create", "Appointment", new { ServiceId = parsedServiceId });
				}
				return RedirectToAction("Index", "Home");
			}

			// Eğer giriş başarısızsa hata mesajı göster
			ViewBag.ErrorMessage = "Invalid email or password.";
			return View();
		}



		// Handle registration form submission
		[HttpPost]
		public IActionResult Register(Customer customer)
		{
			if (ModelState.IsValid)
			{
				// Check if the email already exists
				var existingUser = _context.Customers.FirstOrDefault(u => u.Email == customer.Email);
				if (existingUser != null)
				{
					ViewBag.ErrorMessage = "An account with this email already exists.";
					return View(customer);
				}

				// Add new customer to the database
				_context.Customers.Add(customer);
				_context.SaveChanges();

				// Automatically log in the user after registration
				HttpContext.Session.SetString("UserEmail", customer.Email);
				HttpContext.Session.SetInt32("UserId", customer.CustomerId);

				// Check if serviceId exists in session and redirect accordingly
				var serviceId = HttpContext.Session.GetString("ServiceId");

				// Try to parse the serviceId to an integer
				int parsedServiceId;
				bool isValidServiceId = int.TryParse(serviceId, out parsedServiceId);

				// If serviceId is valid and non-zero, redirect to the Appointment creation page
				if (isValidServiceId && parsedServiceId > 0)
				{
					return RedirectToAction("Create", "Appointment", new { ServiceId = parsedServiceId });
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}
			}

			// If registration fails (e.g., invalid input), show errors
			return View(customer);
		}


		public IActionResult SignOut()
		{
			// Session'ı sonlandır
			HttpContext.Session.Remove("UserEmail");
			HttpContext.Session.Remove("UserRole");
			HttpContext.Session.Remove("UserId");


			// Kullanıcıyı ana sayfaya yönlendir
			return RedirectToAction("Index", "Home");
		}
	}
}
