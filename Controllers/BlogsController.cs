using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheBlogProject.Data;
using TheBlogProject.Models;
using TheBlogProject.Services;

namespace TheBlogProject.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;

        public BlogsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Blogs.Include(b => b.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create

        //Can have multiple roles
        //[Authorize(Roles = "Admin,Moderator")]

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //Alternate way to store data besides a model
            //Transfer data from controller to view
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Image")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now;
                blog.BlogUserId = _userManager.GetUserId(User);
                blog.ImageData = await _imageService.EncodeImageAsync(blog.Image);
                blog.ContentType = _imageService.ContentType(blog.Image);
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Blog blog, IFormFile newImage)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newBlog = await _context.Blogs.FindAsync(blog.Id);

                    newBlog.Updated = DateTime.Now;

                    if (!newBlog.Name.Equals(blog.Name))
                    {
                        newBlog.Name = blog.Name;
                    }

                    if (!newBlog.Description.Equals(blog.Description))
                    {
                        newBlog.Description = blog.Description;
                    }

                    if (newImage != null)
                    {
                        newBlog.ImageData = await _imageService.EncodeImageAsync(newImage);
                    }

                    //_context.Update(blog); //take out because it requires us to pass all of the information that we want preserved into the post from the form.
                    await _context.SaveChangesAsync();//will just save the changes that were made to the record
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
