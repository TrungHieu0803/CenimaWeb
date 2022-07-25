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

namespace CenimaApp.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;

        public DetailsModel(CenimaApp.Models.CenimaDBContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; }
        public IList<Rate> Rates { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            }else
            {
                return RedirectToPage("/Movies/Index");
            }
            Person person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            if (person.Type == 1)
            {
                return RedirectToPage("/Movies/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies
                .Include(m => m.Genre).FirstOrDefaultAsync(m => m.MovieId == id);
            Rates = await _context.Rates.Include(p => p.Person).Where(s => s.MovieId == Movie.MovieId).ToListAsync();
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public JsonResult OnGetRevenueData()
        {
            int movieId = (int)TempData["movieId"];
            DateTime? now = DateTime.Now;
            int[] start = new int[10];
            int[] rates = new int[10];
            for (int i = 0; i <= 9; i++)
            {
                start[i] = i + 1;
                rates[i] = _context.Rates.Where(s => s.MovieId == 1 && s.NumericRating == start[i]).ToList().Count();
            }

            return new JsonResult(new { rate = rates, start = start });
        }

        public JsonResult OnGetRate()
        {
            List<Rate> r = new List<Rate>();
            int movieId = 1;
            r = _context.Rates.Include(s => s.Person).Where(p => p.MovieId == movieId).ToList();
            string[] comment = new string[r.Count];
            string[] name = new string[r.Count];
            double[] rate = new double[r.Count];
            for (int i = 0; i < r.Count; i++)
            {
                comment[i] = r[i].Comment;
                name[i] = r[i].Person.Fullname;
                rate[i] =(double) r[i].NumericRating;
            }

            return new JsonResult(new {comment = comment, rate = rate, fullName = name});
        }
    }
}
