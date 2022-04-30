using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfu.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ParfuContext context;

        public CategoriesViewComponent(ParfuContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
