using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class CredentialDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int? SubjectId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }

        public string Status { get; set; }

    }
}
