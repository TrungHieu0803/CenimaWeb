using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CenimaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CenimaApp.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly CenimaApp.Models.CenimaDBContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(CenimaApp.Models.CenimaDBContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public PaginatedList<Movie> Movies { get; set; }
        public async Task OnGetAsync(string searchString, int? pageIndex)
        {
           
            IQueryable<Movie> movieIQ;
            var pageSize = Configuration.GetValue("PageSize", 3);
            if (searchString == null || searchString == "")
            {
                movieIQ = from m in _context.Movies
               .Include(m => m.Genre)
                          select m;
                Movies = await PaginatedList<Movie>.CreateAsync(movieIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            else
            {

                movieIQ = from m in _context.Movies
              .Include(m => m.Genre).Where(s => s.Title.ToLower().Contains(searchString.ToLower()))
                          select m;
                IQueryable<Movie> movieQ = movieIQ.Where(s => s.Genre.Description.ToLower().Contains(searchString.ToLower()));
                Movies = await PaginatedList<Movie>.CreateAsync(movieQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            if (HttpContext.Session.GetString("account") != null)
                TempData["User"] = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("account"));
        }
    }
}
