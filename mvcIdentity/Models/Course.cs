using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcIdentity.Models
{
    public class Course
    {
        [Key]
        public int Crs_Id { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Course Name")]
        public string Crs_Name { get; set; }
        [StringLength(150)]
        [Display(Name = "Course Description")]
        [AllowHtml]
        public string Crs_Description { get; set; }
        [Display(Name = "Course Duration")]
        public int? Crs_Duration { get; set; }
        [Display(Name = "Topic Name")]
        public int Top_Id { get; set; }
        [Display(Name = "Course Photo")]
        public string Crs_Photo { get; set; }
        [Display(Name = "Course Price")]
        public int? Crs_Price { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ApplicationUser user { get; set; }

        //public virtual ICollection<Ins_Course> Ins_Course { get; set; }
        //public virtual ICollection<Stud_Course> Stud_Course { get; set; }
    }
}