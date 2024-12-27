using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
    public class BookController : Controller
    {
        // Action method to handle the booking for a specific service
        public IActionResult BookService(string serviceName)
        {
            // Based on the serviceName, you can customize the logic to handle different services.
            // For now, we'll just pass the serviceName to a view for simplicity.

            ViewBag.ServiceName = serviceName;

            // Return the booking view with the serviceName
            return View();
        }
    }
}
