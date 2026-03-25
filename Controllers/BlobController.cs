
using Microsoft.AspNetCore.Mvc;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController(BlobServices blobServices) : ControllerBase
    {
        private readonly BlobServices _BlobServices = blobServices;

        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string fileName)
        {
            if(file == null || file.Length == 0 ) return BadRequest("Invalid File");

            using var stream = file.OpenReadStream();

            var fileUrl = await _BlobServices.UploadFileAsync(stream, fileName);

            return Ok( new { FileUrl = fileUrl });
        }
    }
}