using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CenimaApp.Pages.Peoples
{
    public class IndexModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public IndexModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }

        public List<Person> Person { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            }
            else
            {
                return RedirectToPage("/Movies/Index");
            }
            Person person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            if (person.Type == 1)
            {
                return RedirectToPage("/Movies/Index");
            }
            Person = _context.Persons.ToList();
            return Page();
        }
    }
}
