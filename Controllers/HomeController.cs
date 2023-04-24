using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Models;
using TheBlogProject.Services;
using TheBlogProject.ViewModels;
using X.PagedList;

namespace TheBlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db,ILogger<HomeController> logger, IBlogEmailSender emailSender = null)
        {
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int currPage = page ?? 1;
            int pageSize = 3;
            var blogs = _db.Blogs.Include(b => b.BlogUser).OrderByDescending(b => b.Created).ToPagedListAsync(page, pageSize);
            //var blogs = _db.Blogs.Where(x => x.Posts.Any(p => p.ReadyStatus == Enums.ReadyStatus.ProductionReady)).OrderByDescending(b => b.Created).ToPagedListAsync(pageNum,pageSize);

            return View(await blogs);
            //var blogs = await _db.Blogs
            //    .Include(b => b.BlogUser)
            //    .ToListAsync();

            //return View(blogs);
            
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            //Emailing will be taking place here
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";

            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return RedirectToAction("Index");

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //Service is just an C# class that can be called upon to do certain units of work.
    }
}
