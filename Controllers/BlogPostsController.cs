﻿using System;
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

namespace DailyRoarBlog.Controllers
{
    [Authorize(Roles = "Admin")]

    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public BlogPostsController(ApplicationDbContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogPosts.Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> AdminPage()
        {
            var blogPosts = await _context.BlogPosts.Include(b => b.Category).ToListAsync();
            return View(blogPosts);
        }


        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            string? userId = _userManager.GetUserId(User);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            // add elements to include, multiselect to include all 
            ViewData["TagList"] = new MultiSelectList(_context.Tags, "Id", "Name");
            //foreach (var emailAddress in email.Split(";"))
            //{
            //    newEmail.To.Add(MailboxAddress.Parse(emailAddress));
            //}

            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,ImageData,ImageType,CategoryId")] BlogPost blogPost)
        {   //TODO Add slug

            if (ModelState.IsValid)
            {
                blogPost.Created = DateTime.UtcNow;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            // add elements to include, multiselect to include all 
            ViewData["TagList"] = new MultiSelectList(_context.Tags, "Id", "Name");
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

                    //TODO Image Service

                    //TODO Slug BlogPost


                    _context.Update(blogPost);

                    //TODO Edit tags

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.BlogPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
            }
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
          return (_context.BlogPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
