using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfu.Infrastructure;
using Parfu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfu.Controllers
{
    public class PagesController : Controller
    {
        private readonly ParfuContext context;
        //constructor
        public PagesController(ParfuContext context)
        {
            this.context = context; //ca sa le pot folosi context sau metode sau actions
        }
        //GET/ or/ slug
        public async Task<IActionResult> Page(string slug)
        {
            if(slug == null)
            {
                return View(await context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if(page ==null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
