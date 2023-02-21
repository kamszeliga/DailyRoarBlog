using X.PagedList;

namespace DailyRoarBlog.Models.ViewModels
{
    public class BlogPostCategoryViewModel
    {
        public IPagedList<BlogPost>? Posts { get; set; }
        public Category? Category { get; set; }
    }
}
