using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
	public class ServicesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
