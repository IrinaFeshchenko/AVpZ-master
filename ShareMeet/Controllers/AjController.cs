using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareMeet.Models;

namespace ShareMeet.Controllers
{
    [Route("/api/product")]
    public class AjController : Controller
    {
        private UsersContext db;
        public AjController(UsersContext context)
        {
            db = context;
        }
        [HttpGet("findall")]
        public IActionResult InfoMeeting(int id)
        {
            var meetings = from meet in db.MeetUps
                           where meet.Id_meetup == id
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
            ViewBag.meet = meetings.ToList();
            return View();
        }
    }
}
