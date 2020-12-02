using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareMeet.Models;
using ShareMeet.ViewModels;

namespace ShareMeet.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UsersContext db;

        public ProfileController(UsersContext context)
        {
            db = context;
        }

        public IActionResult Profile()
        {

            var Profile = db.Users.Where(p => p.Email == User.Identity.Name);

            var userId = db.Users.Where(p => p.Email == User.Identity.Name);
            int Id = 0;
            foreach (User user in userId)
                Id = user.Id;
            
            var companies = from user in db.Users
                            from comp in db.Companies
                            where user.Id == Id && Id == comp.Id_user
                            select new Company
                            {
                                Id_company = comp.Id_company,
                                Name=comp.Name,
                                Description=comp.Description
                            };
            var meetings = from user in db.Users
                           from comp in db.Companies
                           from meet in db.MeetUps
                           where user.Id==Id && Id==comp.Id_user && comp.Id_company == meet.companyId_company
                               select new MeetUp
                               {
                                   Id_meetup=meet.Id_meetup,
                                   Name=meet.Name,
                                   Type=meet.Type,
                                   Description=meet.Description,
                                   companyId_company=meet.companyId_company,
                                   StartofSelection=meet.StartofSelection,
                                   FinishofSelection=meet.FinishofSelection,
                                   lng=meet.lng,
                                   lat=meet.lat,
                                   Adress=meet.Adress,
                                   Cost=meet.Cost
                                   
                               };
            ViewBag.data = Profile.ToList();
            ViewBag.company = companies.ToList();
            ViewBag.meetings = meetings.ToList();
            return View();
        }

        [HttpGet]
        public IActionResult New_Comp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New_Comp(NewMeetUp model)
        {
            if (ModelState.IsValid)
            {
                var Profile = db.Users.Where(p => p.Email == @User.Identity.Name);
                int id = 0;
                foreach (User user in Profile)
                    id = user.Id;
                Company new_comp = new Company
                {
                    Name = model.Name,
                    Description=model.Description,
                    Id_user=id
                };

                db.Companies.Add(new_comp);
                db.SaveChanges();
                return RedirectToAction("Profile", "Profile");
            }
            return View(model);

        }

        public async Task<IActionResult> DeleteCompany(int? company_id)
        {
            Company delete = await db.Companies.FirstOrDefaultAsync(p => p.Id_company == company_id);
            MeetUp meetup = await db.MeetUps.FirstOrDefaultAsync(p => p.companyId_company == company_id);
            db.Companies.Remove(delete);
            await db.SaveChangesAsync();
            if(meetup!=null)
            {
                db.MeetUps.Remove(meetup);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Profile", "Profile");
        }



        [HttpGet]
        public IActionResult New_MeetUp(int? id)
        {
            ViewBag.companyId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New_MeetUp(NewMeetUp model)
        {
            if (ModelState.IsValid)
            {
                var Profile = db.Users.Where(p => p.Email == @User.Identity.Name);
                int id = 0;
                foreach (User user in Profile)
                    id = user.Id;
                MeetUp new_meet = new MeetUp { Name=model.Name,Type=model.Type,Description=model.Description,companyId_company=model.companyId_company,StartofSelection=model.StartofSelection,
                FinishofSelection=model.FinishofSelection,lng=model.lng,lat=model.lat,Adress=model.Adress,Cost=model.Cost};

                db.MeetUps.Add(new_meet);
                db.SaveChanges();
                return RedirectToAction("Profile", "Profile");
            }
            return View(model);

        }


        public async Task<IActionResult> Delete(int? id,int? company_id)
        {
            int id_user = 0;
            var Profile = db.Users.Where(p => p.Email == @User.Identity.Name);
            foreach (User user in Profile)
                id_user = user.Id;
            MeetUp delete = await db.MeetUps.FirstOrDefaultAsync(p => p.Id_meetup ==id  && p.companyId_company == company_id);
            db.MeetUps.Remove(delete);
            await db.SaveChangesAsync();
            return RedirectToAction("Profile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Edit_my_meetup(int? id)
        {
            MeetUp meet = await db.MeetUps.FirstOrDefaultAsync(p => p.Id_meetup == id);
            return View(meet);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_my_meetup(MeetUp model)
        {
            MeetUp edit_meet = new MeetUp { Id_meetup=model.Id_meetup,Name = model.Name, Type = model.Type,Description=model.Description,companyId_company=model.companyId_company,
                StartofSelection=model.StartofSelection,FinishofSelection=model.FinishofSelection,lng=model.lng,lat=model.lat,Adress=model.Adress,Cost=model.Cost };
            db.MeetUps.Update(edit_meet);
            await db.SaveChangesAsync();
            return RedirectToAction("Profile", "Profile");
        }
        [HttpGet]
        public async Task<IActionResult> Edit_my_company(int? id)
        {
            Company comp = await db.Companies.FirstOrDefaultAsync(p => p.Id_company == id);
            return View(comp);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_my_company(Company model)
        {
            Company edit_comp = new Company
            {
                Id_company=model.Id_company,
                Name=model.Name,
                Description=model.Description,
                Id_user=model.Id_user
            };
            db.Companies.Update(edit_comp);
            await db.SaveChangesAsync();
            return RedirectToAction("Profile", "Profile");
        }
    }
}
