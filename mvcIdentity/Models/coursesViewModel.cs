using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcIdentity.Models
{
    public class coursesViewModel
    {
        public string courseTitle { get; set; }
        public IEnumerable<ApplyForCourse> items { get; set; }
    }
}