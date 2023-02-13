﻿

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyRoarBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? Title { get; set; }

        [StringLength(600, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? Abstract { get; set; }

        [Required]
        public string? Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }
        
        //TODO Make Slug Required
        public string? Slug { get; set; }

        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }
        
        [Display(Name = "Published?")]
        public bool IsPublished { get; set; }

        //image properties
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        public virtual IFormFile? ImageFile { get; set; }


        //Navigation properties

        public int CategoryId { get; set; }  //foriegn key
        public virtual Category? Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>(); //many to many

        public virtual ICollection<Comment> Comments { get;} = new HashSet<Comment>(); // one to many


    }
}
