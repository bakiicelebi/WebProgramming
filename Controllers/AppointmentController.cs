using Microsoft.AspNetCore.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly DataBaseContext db;

		public AppointmentController(DataBaseContext db)
		{
			this.db = db;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
