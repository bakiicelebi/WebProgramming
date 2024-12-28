using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Models;
using System.Linq;
using System;
using System.Collections.Generic;

public class AppointmentController : Controller
{
    private readonly DataBaseContext _context;

    public AppointmentController(DataBaseContext context)
    {
        _context = context;
    }

    // Service selection page
    public IActionResult Create(int serviceId)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            HttpContext.Session.SetString("ServiceId", serviceId.ToString());
            return RedirectToAction("Index", "Login");
        }

        Appointment pendingAppointment = null;

        var customerEmail = HttpContext.Session.GetString("UserEmail");
        var customer = _context.Customers.FirstOrDefault(c => c.Email == customerEmail);

        if(customer != null)
        {
			if (customer.CustomerId != null)
			{
				pendingAppointment = _context.Appointments
				.FirstOrDefault(a => a.CustomerId == customer.CustomerId);
			}
		}

        var service = _context.Services.Include(s => s.Salon).FirstOrDefault(s => s.ServiceId == serviceId);
        if (service == null)
        {
            ViewBag.ErrorMessage = "Service not found!";
            return View();
        }

        // Pass service info to the view
        ViewBag.PendingAppointment = pendingAppointment;
        ViewBag.ServiceId = serviceId;
        ViewBag.ServiceName = service.Name;
        ViewBag.Salon = service.Salon;
        ViewBag.ServicePrice = service.Price;
        ViewBag.ServiceDuration = service.Duration;

        // Get employees for the salon
        ViewBag.Employees = _context.Employees.Where(e => e.SalonId == service.SalonId).ToList();

        return View();
    }

    [HttpPost]
    public IActionResult GetAvailableDates(int employeeId, int serviceId)
    {
        // Çalışanın bu servis için aldığı randevu tarihlerinin listesini getir
        var availableDates = _context.Appointments
            .Where(a => a.EmployeeId == employeeId
                        && a.ServiceId == serviceId
                        && a.AppointmentTime > DateTime.Now) // Geçmiş tarihleri çıkar
            .Select(a => a.AppointmentTime.Date) // Sadece tarihi al
            .Distinct()
            .ToList();

        // Tarih formatını uygun şekilde dön
        return Json(availableDates.Select(d => d.ToString("MM/dd/yyyy")));
    }
    [HttpPost]
    public IActionResult GetAvailableTimes(int employeeId, int serviceId, DateTime date)
    {
        var workingHoursStart = TimeSpan.FromHours(9); // Çalışma saati başlangıcı
        var workingHoursEnd = TimeSpan.FromHours(18); // Çalışma saati bitişi

        // O gün için mevcut randevuları al
        var reservedAppointments = _context.Appointments
            .Where(a => a.EmployeeId == employeeId
                        && a.ServiceId == serviceId
                        && a.AppointmentTime.Date == date.Date) // Sadece seçilen günü kontrol et
            .Select(a => new { a.AppointmentTime.TimeOfDay, a.CustomerId }) // Zaman ve müşteri bilgilerini al
            .ToList();

        // Çalışma saatleri arasında uygun zamanları bul
        var availableTimes = new List<object>();
        for (var currentTime = workingHoursStart; currentTime < workingHoursEnd; currentTime += TimeSpan.FromHours(1))
        {
            var isReserved = reservedAppointments.Any(a => a.TimeOfDay == currentTime && a.CustomerId != null);
            availableTimes.Add(new
            {
                time = currentTime.ToString(@"hh\:mm"),
                isReserved
            });
        }

        return Json(availableTimes);
    }



    [HttpPost]
	public IActionResult BookAppointment(int employeeId, int serviceId, string appointmentTime)
	{
		if (!DateTime.TryParse(appointmentTime, out var parsedAppointmentTime))
		{
			return Json(new { success = false, message = "Invalid date and time format." });
		}

		var customerEmail = HttpContext.Session.GetString("UserEmail");
		var customer = _context.Customers.FirstOrDefault(c => c.Email == customerEmail);

		if (customer == null)
		{
			return Json(new { success = false, message = "Customer not found." });
		}

		var appointment = _context.Appointments.FirstOrDefault(a =>
			a.EmployeeId == employeeId && a.ServiceId == serviceId && a.AppointmentTime == parsedAppointmentTime && a.CustomerId == null);

		if (appointment == null)
		{
			return Json(new { success = false, message = "Appointment not available." });
		}

		// Appointment güncelleniyor
		appointment.CustomerId = customer.CustomerId;
		appointment.IsConfirmed = null; // Admin daha sonra onaylayacak
		_context.SaveChanges();

		return Json(new { success = true, message = "Appointment booked successfully!" });
	}

	public IActionResult EditAppointment()
	{
		var appointments = _context.Appointments
			 .Where(a => a.CustomerId != null) // Null olmayan CustomerId'li randevuları al
			 .ToList();

		return View(appointments);
	}
	// Onaylama işlemi
	[HttpPost]
	public IActionResult Approve(int id)
	{
		var appointment = _context.Appointments.Find(id);
		if (appointment != null)
		{
			appointment.IsConfirmed = true;  // Onaylandı
			_context.SaveChanges();
		}

        return RedirectToAction("EditAppointment", "Appointment");
    }

	// İptal etme işlemi
	[HttpPost]
	public IActionResult Cancel(int id)
	{
		var appointment = _context.Appointments.Find(id);
		if (appointment != null)
		{
			appointment.IsConfirmed = false;  // Reddedildi
			_context.SaveChanges();
		}

		return RedirectToAction("EditAppointment","Appointment");
	}


}
