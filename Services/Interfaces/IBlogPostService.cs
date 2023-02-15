using DailyRoarBlog.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DailyRoarBlog.Services.Interfaces
{
    public interface IBlogPostService
    {
        // CRUD Services

        public Task AddBlogPostAsync(BlogPost blogPost);

        public Task UpdateBlogPostAsync(BlogPost blogPost);

        public Task<BlogPost> GetBlogPostAsync(int blogPostId);

        public Task DeleteBlogPostAsync(BlogPost blogPost);

        // Get BlogPOSTS Methods

        public Task<IEnumerable<BlogPost>> GetBlogPostsAsync();

        public Task<IEnumerable<BlogPost>> GetPopularPostsAsync();
        public Task<IEnumerable<BlogPost>> GetPopularPostsAsync(int count);

        public Task<IEnumerable<BlogPost>> GetRecentPostsAsync();
        public Task<IEnumerable<BlogPost>> GetRecentPostsAsync(int count);


        // Categories

        public Task AddCategoryAsync(Category category);

        public Task UpdateCategoryAsync(Category category);

        public Task<Category> GetCategoryAsync(int categoryId);

        public Task<IEnumerable<Category>> GetCategoriesAsync(int categoryId);

        public Task DeleteCategoryAsync(Category category);

    }
}
