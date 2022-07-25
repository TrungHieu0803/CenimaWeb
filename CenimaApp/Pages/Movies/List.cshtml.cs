using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CenimaApp.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CenimaApp.Pages.Movies
{
    public class ListModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;
        private readonly IConfiguration Configuration;

        public ListModel(CenimaApp.Models.CenimaDBContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public PaginatedList<Movie> Movies { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));

            }else
            {
                return RedirectToPage("/Movies/Index");
            }
            IQueryable<Movie> movieIQ;
            var pageSize = Configuration.GetValue("PageSize", 3);
            movieIQ = from m in _context.Movies
              .Include(m => m.Genre).Include(s => s.Rates)
                      select m;

            Movies = await PaginatedList<Movie>.CreateAsync(movieIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}

