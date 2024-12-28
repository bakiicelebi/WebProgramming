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

    // Randevu oluşturma sayfasını göster
    public IActionResult Create(int serviceId)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            HttpContext.Session.SetString("ServiceId", serviceId.ToString());
            return RedirectToAction("Index", "Login");
        }

        var service = _context.Services.Include(s => s.Salon).FirstOrDefault(s => s.ServiceId == serviceId);
        if (service == null)
        {
            ViewBag.ErrorMessage = "Service not found!";
            return View();
        }

        // ViewBag'de gerekli veriler
        ViewBag.ServiceId = serviceId;
        ViewBag.ServiceName = service.Name;
        ViewBag.Salon = service.Salon;
        ViewBag.ServicePrice = service.Price;
        ViewBag.ServiceDuration = service.Duration;

        // Çalışanlar listesi
        ViewBag.Employees = _context.Employees.Where(e => e.SalonId == service.SalonId).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult GetAvailableDates(int employeeId, int serviceId)
    {
        try
        {
            var dates = _context.Appointments
                .Where(a => a.EmployeeId == employeeId && a.ServiceId == serviceId && a.AppointmentTime > DateTime.Now && !a.IsConfirmed)
                .Select(a => a.AppointmentTime.Date.ToString("MM/dd/yyyy"))
                .Distinct()
                .ToList();

            return Json(dates);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching dates: " + ex.Message);
            return Json(new { success = false, message = "Error fetching available dates." });
        }
    }

    [HttpPost]
    public IActionResult GetAvailableTimes(int employeeId, int serviceId, DateTime date)
    {
        var workingHoursStart = TimeSpan.FromHours(9);
        var workingHoursEnd = TimeSpan.FromHours(18);

        var dayStart = date.Date + workingHoursStart;
        var dayEnd = date.Date + workingHoursEnd;

        var reservedAppointments = _context.Appointments
            .Where(a => a.EmployeeId == employeeId && a.ServiceId == serviceId && a.AppointmentTime >= dayStart && a.AppointmentTime < dayEnd && a.IsConfirmed)
            .ToList();

        var availableTimes = new List<string>();
        for (var currentTime = workingHoursStart; currentTime < workingHoursEnd; currentTime += TimeSpan.FromHours(1))
        {
            var appointmentTime = date.Date + currentTime;

            if (!reservedAppointments.Any(a => a.AppointmentTime == appointmentTime))
            {
                availableTimes.Add(appointmentTime.ToString("HH:mm"));
            }
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
            a.EmployeeId == employeeId && a.ServiceId == serviceId && a.AppointmentTime == parsedAppointmentTime && !a.IsConfirmed);

        if (appointment == null)
        {
            return Json(new { success = false, message = "Appointment not available." });
        }

        appointment.CustomerId = customer.CustomerId;
        appointment.IsConfirmed = true;
        _context.SaveChanges();

        return Json(new { success = true, message = "Appointment booked successfully!" });
    }


}
