using LMS.Application.Courses.DTOs;
using LMS.Application.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCourseByIdQuery(id),
            cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

}