using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebProject.Models;

namespace WebProject.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly DataBaseContext _context;

		public EmployeeController(DataBaseContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			// Çalışan listesini veritabanından alıyoruz
			var employees = _context.Employees.ToList();
			return View(employees); // View'e listeyi gönderiyoruz
		}
	}
}
