using DailyRoarBlog.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DailyRoarBlog.Services.Interfaces
{
    public interface IBlogPostService
    {
        // CRUD Services

        public Task AddBlogPostAsync(BlogPost blogPost);

        public Task UpdateBlogPostAsync(BlogPost blogPost);
        /// <summary>
        /// Get a single BlogPost by Id (integer)
        /// </summary>
        /// <param name="blogPostId"></param>
        /// <returns></returns>

        public Task<BlogPost> GetBlogPostAsync(int blogPostId);
        /// <summary>
        /// Get a single BlogPost by Slu (string)
        /// </summary>
        /// <param name="blogPostSlug"></param>
        /// <returns></returns>
        public Task AddTagsToBlogPostAsync(string stringTags, int blogPostId);
        public Task<BlogPost> GetBlogPostAsync(string blogPostSlug);

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

        public Task<IEnumerable<Category>> GetCategoriesAsync();

        public Task DeleteCategoryAsync(Category category);

        // Additional Methods

        public Task AddTagsToBlogPostAsync(IEnumerable<int> tagIds, int blodPostsId);

        public Task<bool> IsTagOnBlogPostAsync(int tagId, int blogPostId);

        public Task RemoveAllBlogPostTagsAsync(int blogPostId);

        public IEnumerable<BlogPost> SearchBlogPosts(string? searchString);
        public Task<bool> ValidateSlugAsync(string title, int blogId);

        public Task<IEnumerable<Tag>> GetTagsAsync();

        public Task<Tag> GetTagAsync(int tagId);

        public Task AddNewTagAsync(Tag tag);

    }
}
