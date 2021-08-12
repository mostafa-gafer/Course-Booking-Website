using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvcIdentity.Models
{
    public class Topic
    {
        [Key]
         public int Top_Id { get; set; }

        [StringLength(50)]
        [Display(Name ="Topic Name")]
        [Required]
        public string Top_Name { get; set; }
        [Display(Name = "Topic Photo")]
        public string Top_Photo { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}