using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfu.Infrastructure;
using Parfu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Area allows us to partition the large application into smaller units where
//each unit contains a separate MVC folder structure

namespace Parfu.Areas.Admin.Controllers
{
    //controllers in mvc are c# classes that extends controllers
    [Area("Admin")]
    public class PagesController : Controller
    {
        //metoda Index o sa returneze toate pages
        //am nevoie de o dependency pe care sa o injectez in clasa ca sa iau data base context
        //ca sa inject dependencies in controller am nevoie de constructori

        private readonly ParfuContext context;
        //constructor
        public PagesController(ParfuContext context)
        {
            this.context = context; //ca sa le pot folosi context sau metode sau actions
        }

        //GET /admin/ pages
        public async Task<IActionResult> Index() //IactionResults catch all for returns
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;
            List<Page> pagesList = await pages.ToListAsync();
            return View(pagesList);
        }


        //GET /admin/ pages / details / 5
        public async Task<IActionResult> Details(int id) //id e route parameter din Index.cshtml
        {
            //to get a specific page

            Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        //GET /admin/ pages / create 
        public IActionResult Create() => View();


        //POST / admin/ pages/ create
        [HttpPost]
        public async Task<IActionResult> Create(Page page) //id e route parameter din Index.cshtml
        {
            if(ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);

                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Succes"] = "The page has been added!";
                return RedirectToAction("Index");


            }

            return View(page);
        }

        //GET /admin/ pages / edit / 5
        public async Task<IActionResult> Edit(int id) //id e route parameter din Index.cshtml
        {
            //to get a specific page

            Page page = await context.Pages.FindAsync( id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }


        //POST / admin/ pages/ edit
        [HttpPost]
        public async Task<IActionResult> Edit(Page page) //id e route parameter din Index.cshtml
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id ==1 ? "home" : page.Title.ToLower().ToLower().Replace(" ", "-");

                

                var slug = await context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);

                }

                context.Update(page);
                await context.SaveChangesAsync();

                TempData["Succes"] = "The page has been edited!";
                return RedirectToAction("Edit", new { id = page.Id });


            }

            return View(page);
        }


        //GET /admin/ pages / delete/ 5
        public async Task<IActionResult> Delete(int id) //id e route parameter din Index.cshtml
        {
           

            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exists!";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been deleted!";
            }
            return RedirectToAction("Index");
        }

    }
}
