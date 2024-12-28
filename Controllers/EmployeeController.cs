using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
using WebProject.Data; // Eğer gerekli ise
using System.Linq;

namespace WebProject.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly DataBaseContext _context;

		public EmployeeController(DataBaseContext context)
		{
			_context = context;
		}

		// Index - Listeleme
		public IActionResult Index()
		{
			var employees = _context.Employees.ToList();
			return View(employees);
		}

		public IActionResult EditEmployees()
		{
			// Çalışanlar ve onların randevu sayıları ile birlikte veritabanından alınıyor
			var employees = _context.Employees
				.Select(e => new Employee
				{
					EmployeeId = e.EmployeeId,
					Name = e.Name,
					Position = e.Position,
					ProfileImageUrl = e.ProfileImageUrl,
					// Appointment sayısı almak için Appointments tablosunu sayıyoruz
					Appointments = e.Appointments, // Bu satır gerekli değil, sadece sayısal veriye ihtiyaç var
				})
				.ToList()
				.Select(e => new
				{
					e.EmployeeId,
					e.Name,
					e.Position,
					e.ProfileImageUrl,
					AppointmentCount = _context.Appointments.Count(a => a.EmployeeId == e.EmployeeId && a.IsConfirmed == true)
				})
				.ToList();

			return View(employees); // Verileri view'e gönderiyoruz
		}
		public IActionResult Delete(int id)
		{
			var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
				_context.SaveChanges();
			}

			return RedirectToAction("EditEmployees", "Employee");
		}

		// GET: Employee/Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    return RedirectToAction("EditEmployees");
                }
                catch (Exception ex)
                {
                    // Daha anlamlı bir mesaj gösterelim
                    ModelState.AddModelError("", "Veritabanına kaydedilirken bir hata oluştu: " + ex.Message);
                    return View(employee);
                }
            }
            else
            {
                // ModelState geçerli değilse, hata mesajlarını logluyoruz
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Burada hata mesajlarını ekranda gösterebiliriz
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(employee);
        }


    }
}
