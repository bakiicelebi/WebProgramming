using Microsoft.AspNetCore.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataBaseContext _context;

        public ServicesController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            // Veritabanından servisleri al
            var services = _context.Services.ToList();


            return View(services);
        }
    }
}
