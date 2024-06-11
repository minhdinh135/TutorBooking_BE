using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models
{
    public partial class Post : BaseModel
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public int GradeLevel { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ContactInfo { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
    }
}
