using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvcIdentity.Models
{
    public class ApplyForCourse
    {
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        [ForeignKey("course")]
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public virtual Course course { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}