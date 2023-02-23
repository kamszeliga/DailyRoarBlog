using DailyRoarBlog.Data;
using DailyRoarBlog.Models;
using DailyRoarBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace DailyRoarBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBlogPostService _blogPostService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IEmailSender _emailSender;


        public HomeController(ILogger<HomeController> logger, 
                                ApplicationDbContext context, 
                                IBlogPostService blogPostService, 
                                UserManager<BlogUser> userManager,
                                IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _blogPostService = blogPostService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(int? pageNum)
        {
            int pageSize = 3;
            int page = pageNum ?? 1;


            IPagedList<BlogPost> model = (await _blogPostService.GetRecentPostsAsync()).ToPagedList(page, pageSize);

            return View(model);
        }

        public IActionResult SearchIndex(string? searchString, int? pageNum) 
        {
            int pageSize = 5;
            int page = pageNum ?? 1;


            IPagedList<BlogPost> model = ( _blogPostService.SearchBlogPosts(searchString)).ToPagedList(page, pageSize);

            return View(nameof(Index), model);


        }

        public IActionResult ContactMe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // EmailContact page
        //public async Task<IActionResult> EmailAdmin(int? id)
        //{

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    string? appUserId = _userManager.GetUserId(User)!;

        //    Contact? contact = await _context.Contacts
        //                                .Where(c => c.AppUserID == appUserId)
        //                                .FirstOrDefaultAsync(c => c.Id == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    // Instantiate EmailData
        //    EmailData emailData = new EmailData()
        //    {
        //        EmailAddress = contact!.Email,
        //        FirstName = contact.FirstName,
        //        LastName = contact.LastName,
        //    };

        //    // Instantiate View Model
        //    EmailContactViewModel viewModel = new EmailContactViewModel()
        //    {
        //        Contact = contact,
        //        EmailData = emailData,
        //    };

        //    return View(viewModel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailAdmin()
        {
            if (ModelState.IsValid)
            {
                string? swalMessage = string.Empty;


                try
                {
                    //await _emailSender.SendEmailAsync(
                    //                        viewModel.EmailData!.EmailAddress!,
                    //                        viewModel.EmailData.EmailSubject!,
                    //                        viewModel.EmailData.EmailBody!);

                    swalMessage = "Your email has been sent.";

                    return RedirectToAction(nameof(Index), new { swalMessage });
                }
                catch (Exception)
                {
                    swalMessage = "Error: Email failed to send.";

                    return RedirectToAction(nameof(Index), new { swalMessage });

                    throw;
                }
            }

            return View();
        }


    }
}