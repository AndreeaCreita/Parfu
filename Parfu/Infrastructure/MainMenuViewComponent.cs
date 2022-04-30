using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfu.Infrastructure
{
    //you get a dependency through the constructor
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly ParfuContext context;

        public MainMenuViewComponent(ParfuContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
