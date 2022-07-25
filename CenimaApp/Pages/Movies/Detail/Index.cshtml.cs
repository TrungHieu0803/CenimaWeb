using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using CenimaApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.SignalR;

namespace CenimaApp.Pages.Movies.Detail
{
    public class IndexModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;
        private readonly IHubContext<SignLRServer> _signal;
        public IndexModel(CenimaApp.Models.CenimaDBContext context, IHubContext<SignLRServer> signal)
        {
            _context = context;
            _signal = signal;
        }

        public Movie movie { get; set; }
        public double? rate;
        public Person person { get; set; }

        [BindProperty]
        public Rate personRate { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("account") != null)
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("account") != null)
            {
                person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
                personRate = _context.Rates.SingleOrDefault(r => r.PersonId == person.PersonId && r.MovieId == id);
            }
            movie = _context.Movies.Include(s => s.Genre).Include(s => s.Rates).SingleOrDefault(m => m.MovieId == id);
            rate = movie.Rates.Select(s => s.NumericRating).Sum() / movie.Rates.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));

            }else
            {
                return RedirectToPage("/Movies/Index");
            }
            movie = _context.Movies.Include(s => s.Genre).Include(s => s.Rates).SingleOrDefault(m => m.MovieId == personRate.MovieId);
            person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
            Rate checkRate = _context.Rates.SingleOrDefault(r => r.MovieId == personRate.MovieId && r.PersonId == personRate.PersonId);
            DateTime localDate = DateTime.Now;
            if (checkRate == null)
            {
                Rate saveRate = new Rate { NumericRating = personRate.NumericRating, MovieId = movie.MovieId, PersonId = person.PersonId, Time = localDate, Comment = personRate.Comment };
                _context.Rates.Add(saveRate);
                _context.SaveChanges();
            }
            else
            {
                checkRate.Comment = personRate.Comment;
                checkRate.NumericRating = personRate.NumericRating;
                _context.Rates.Update(checkRate);
                _context.SaveChanges();
            }        
            personRate = _context.Rates.SingleOrDefault(r => r.PersonId == person.PersonId && r.MovieId == personRate.MovieId);         
            rate = movie.Rates.Select(s => s.NumericRating).Sum() / movie.Rates.Count;
            await _signal.Clients.All.SendAsync("loadServices");
            return Page();
        }

        public JsonResult OnGetRate()
        {
            List<Rate> r = new List<Rate>();
            int movieId = (int)TempData["movieId"];
            r = _context.Rates.Include(s => s.Person).Where(p => p.MovieId == movieId).ToList();
            string[] comment = new string[r.Count];
            string[] name = new string[r.Count];
            double[] rate = new double[r.Count];
            for (int i = 0; i < r.Count; i++)
            {
                comment[i] = r[i].Comment;
                name[i] = r[i].Person.Fullname;
                rate[i] = (double)r[i].NumericRating;
            }

            return new JsonResult(new { comment = comment, rate = rate, fullName = name });
        }
    }
}
