using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShareMeet.Models;

namespace ShareMeet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UsersContext db;

        public HomeController(ILogger<HomeController> logger,UsersContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            var meetings = from meet in db.MeetUps
                           select new MeetUp
                           {
                               Id_meetup = meet.Id_meetup,
                               Name = meet.Name,
                               Type = meet.Type,
                               Description = meet.Description,
                               companyId_company = meet.companyId_company,
                               StartofSelection = meet.StartofSelection,
                               FinishofSelection = meet.FinishofSelection,
                               lng = meet.lng,
                               lat = meet.lat,
                               Adress = meet.Adress,
                               Cost = meet.Cost

                           };
            var meets = db.MeetUps.Count();
            ViewBag.count = meets;
            ViewBag.data = meetings;
            return View();
        }

        public IActionResult Main()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Pr()
        {
            return View();
        }
        public IActionResult vert()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
