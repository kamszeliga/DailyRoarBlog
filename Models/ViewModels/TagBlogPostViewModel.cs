using X.PagedList;

namespace DailyRoarBlog.Models.ViewModels
{
    public class TagBlogPostViewModel
    {
        public IPagedList<BlogPost>? Posts { get; set; }
        public Tag? Tag { get; set; }
    }
}
