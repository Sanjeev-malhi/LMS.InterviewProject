using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Courses.DTOs
{
    public class CourseResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
