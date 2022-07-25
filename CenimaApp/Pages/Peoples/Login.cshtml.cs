using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CenimaApp.Pages.Peoples
{
    public class LoginModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public LoginModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Person Person { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            var person = _context.Persons.SingleOrDefault(p => p.Email.Equals(Person.Email) && p.Password.Equals(Person.Password));
            if (person != null)
            {
                HttpContext.Session.SetString("account", JsonSerializer.Serialize(person));
                if (person.Type == 1)
                {
                    return RedirectToPage("/Movies/Index");

                }
                else
                {
                    return RedirectToPage("/Admin/Index");
                }
            }
            else
            {
                ViewData["msg"] = "This account not exist.";
                return Page();
            }
        }

    }
}
