using LMS.Application.Courses.DTOs;
using LMS.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Connections.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Courses.Queries.GetCourseById
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;
        public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<CourseResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

            if(course == null)
            {
                return null;
            }

            return new CourseResponse
            {
                Id = course.Id,
                Name = course.Title,
                Description = course.Description,
                Price = course.Price,
            };
        }
    }
}
