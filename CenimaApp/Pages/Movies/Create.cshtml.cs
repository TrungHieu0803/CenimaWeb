using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CenimaApp.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public CreateModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));

            }else
            {
                return RedirectToPage("/Movies/Index");
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Description");
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./List");
        }
    }
}
