﻿using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public static void getIndex()
		{ }
	}
}
