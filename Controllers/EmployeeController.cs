using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
	public class EmployeeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
