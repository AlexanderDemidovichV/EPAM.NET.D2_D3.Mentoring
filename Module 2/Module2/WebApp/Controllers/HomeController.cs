﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ConditionalValidation;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infastructure;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            var visitor = new JavascriptExpressionVisitor();
            ViewBag.Condition = visitor.
                Translate(new RegisterModelValidator().ConvertAllRulesToExpression());
            return View();
        }

        public IActionResult SignIn(RegisterModel registerModel)
        {
            var customValidator = new RegisterModelValidator().Validate(registerModel);

            if (!customValidator.IsValid)
            {
                foreach (var error in customValidator.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            { 
                return new BadRequestResult();
            }

            return Redirect("Register");
        }
    }
}
