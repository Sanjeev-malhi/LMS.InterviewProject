using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public FilesController(
            IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            var url =
                await _blobStorageService.UploadAsync(file);

            return Ok(new
            {
                Message = "File uploaded successfully.",
                Url = url
            });
        }
    }
}