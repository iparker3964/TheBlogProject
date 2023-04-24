using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Enums;
using TheBlogProject.Models;

namespace TheBlogProject.Services
{
    public class BlogSearchService
    {
        private readonly ApplicationDbContext _context;
        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Search(string searchTerm)
        {
            string term = searchTerm.ToLower();

            var posts = _context.Posts.Where(p => p.ReadyStatus == ReadyStatus.ProductionReady).AsQueryable();

            if (searchTerm != null)
            {
                posts = posts.Where(
                        p => p.Title.ToLower().Contains(term) ||
                        p.Abstract.ToLower().Contains(term) ||
                        p.Content.ToLower().Contains(term) ||
                        p.Comments.Any(c => c.Body.ToLower().Contains(term) ||
                                            c.ModeratedBody.ToLower().Contains(searchTerm) ||
                                            c.BlogUser.FirstName.ToLower().Contains(searchTerm) ||
                                            c.BlogUser.LastName.ToLower().Contains(searchTerm) ||
                                            c.BlogUser.Email.ToLower().Contains(searchTerm))
                        );
            }

            return posts.OrderByDescending(x => x.Created);

        }

    }
}
