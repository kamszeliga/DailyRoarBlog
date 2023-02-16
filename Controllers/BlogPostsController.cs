using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DailyRoarBlog.Data;
using DailyRoarBlog.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DailyRoar.Services.Interfaces;
using DailyRoarBlog.Services.Interfaces;
using DailyRoarBlog.Helpers;

namespace DailyRoarBlog.Controllers
{
    [Authorize(Roles = "Admin")]

    public class BlogPostsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IBlogPostService _blogPostService;


        public BlogPostsController(UserManager<BlogUser> userManager, IImageService imageService, IBlogPostService blogPostService)
        {
            //_context = context;
            _userManager = userManager;
            _imageService = imageService;
            _blogPostService = blogPostService;
        }

        //// GET: BlogPosts
        //[AllowAnonymous]
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.BlogPosts.Include(b => b.Category);
        //    return View(await applicationDbContext.ToListAsync());
        //}


        public async Task<IActionResult> AdminPage()
        {
            var blogPosts = await _blogPostService.GetBlogPostsAsync();
            return View(blogPosts);
        }


        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostAsync(slug);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public async Task<IActionResult> CreateAsync()
        {
            string? userId = _userManager.GetUserId(User);

            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");

            return View(new BlogPost());
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,ImageFile,CategoryId")] BlogPost blogPost)
        {  
            if (ModelState.IsValid)
            {
                // Add slug
                if (!await _blogPostService.ValidateSlugAsync(blogPost.Title!, blogPost.Id))
                {
                    ModelState.AddModelError("Title", "This title is already being used.");

                    ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");
                    return View(blogPost);
                }
                blogPost.Slug = StringHelper.BlogSlug(blogPost.Title!);

                // format dates
                blogPost.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);

                //TODO Image Service
                if (blogPost.ImageFile != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                    blogPost.ImageType = blogPost.ImageFile.ContentType;

                }

                //TODO CAll service to save new blog post 

                await _blogPostService.AddBlogPostAsync(blogPost);

                //TODO Add Tags

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostAsync(id.Value);

            if (blogPost == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name", blogPost.CategoryId);
            // add elements to include, multiselect to include all 
            //ViewData["TagList"] = new MultiSelectList(_context.Tags, "Id", "Name");
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,ImageData,ImageType,CategoryId")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Dates Example
                    blogPost.Created = DataUtility.GetPostGresDate(blogPost.Created);
                    blogPost.Updated = DataUtility.GetPostGresDate(DateTime.UtcNow);

                    //Image Service
                    if (blogPost.ImageFile != null)
                    {
                        blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                        blogPost.ImageType = blogPost.ImageFile.ContentType;

                    }
                    // Slug BlogPost
                    // Add slug
                    if (!await _blogPostService.ValidateSlugAsync(blogPost.Title!, blogPost.Id))
                    {
                        ModelState.AddModelError("Title", "This title is already being used.");

                        ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");
                        return View(blogPost);
                    }
                    blogPost.Slug = StringHelper.BlogSlug(blogPost.Title!);

                    //call service to update
                    await _blogPostService.UpdateBlogPostAsync(blogPost);

                    //TODO Edit tags
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BlogPostExists(blogPost.Id))
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
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostAsync(id.Value);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostAsync(id);

            if (blogPost != null)
            {
                await _blogPostService.DeleteBlogPostAsync(blogPost);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogPostExists(int id)
        {
            return (await _blogPostService.GetBlogPostsAsync()).Any(b => b.Id == id);
        }
    }
}
