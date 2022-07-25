using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CenimaApp.Pages.Admin
{
    public class IndexModel : PageModel
    {

        private readonly CenimaApp.Models.CenimaDBContext _context;

        public IndexModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;

        }

        public List<PositivePerson> ps { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToPage("/Movies/Index");
            }
            Person person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));

            List<Person> p = _context.Persons.Include(s => s.Rates).ToList();
            List<PositivePerson> d = new List<PositivePerson>();
            foreach(Person p2 in p)
            {
                d.Add(new PositivePerson { PersonId = p2.PersonId, Email = p2.Email, Fullname = p2.Fullname, Count = p2.Rates.Count });
            }
            ps = d.OrderByDescending(s => s.Count).Take(3).ToList();
            if (person.Type == 1)
            {
                return RedirectToPage("/Movies/Index");
            }
            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = person;

            }

            return Page();

        }
        public JsonResult OnGetRevenueData()
        {
            DateTime? now = DateTime.Now;


            int[] month = new int[12];
            int[] rates = new int[12];
            for (int i = 0; i <= 11; i++)
            {
                month[i] = i + 1;
                rates[i] = _context.Rates.Where(s => s.Time.Month == month[i]).ToList().Count();
            }

            return new JsonResult(new { rate = rates, month = month });
        }
    }

    public class PositivePerson
    {
        public int PersonId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int Count { get; set; }
    }
}
