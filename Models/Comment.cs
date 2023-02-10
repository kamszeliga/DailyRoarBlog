using System.ComponentModel.DataAnnotations;

namespace DailyRoarBlog.Models
{
    public class Comment
    {
        public int Id { get; set; } //primary key

        [Required]
        [Display(Name = "Comment")]
        [StringLength(3000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? Body { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? UpdateReason { get; set; }   

        //Navigation properties
        public int BlogPostId { get; set; } //foriegn key
        public virtual BlogPost? BlogPost { get; set; }

        [Required]
        public string? AuthorId { get; set; } //foriegn key
        public virtual BlogUser? Author { get; set; }


    }
}
