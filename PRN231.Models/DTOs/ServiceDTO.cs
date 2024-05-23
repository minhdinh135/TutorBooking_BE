using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class ServiceDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int SubjectId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public string Content { get; set; }

        public string Duration { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }
    }
}
