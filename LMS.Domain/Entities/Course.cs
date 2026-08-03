    using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ThumbnailUrl { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
