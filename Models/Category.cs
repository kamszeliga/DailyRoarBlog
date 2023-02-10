﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyRoarBlog.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string? Description { get; set; }    


        //image properties
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        public virtual IFormFile? ImageFile { get; set; }

        //Navigation properties

        public virtual ICollection<BlogPost> BlogPosts { get; set; }  = new HashSet<BlogPost>();

    }
}
