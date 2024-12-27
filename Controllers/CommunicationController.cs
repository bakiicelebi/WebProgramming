using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
	public class CommunicationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
