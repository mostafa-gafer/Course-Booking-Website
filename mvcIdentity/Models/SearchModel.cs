using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcIdentity.Models
{
    public class SearchModel
    {
        public List<Course> MyCourses { get; set; }
        public List<ApplicationUser> MyInstructors { get; set; }
        public List<Topic> MyTopics { get; set; }
    }
}