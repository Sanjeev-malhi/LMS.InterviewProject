using LMS.Application.Courses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Courses.Queries.GetCourseById
{
    public record GetCourseByIdQuery(Guid CourseId) : IRequest<CourseResponse>;
}
