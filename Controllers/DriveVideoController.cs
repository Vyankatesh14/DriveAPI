using DriveGoogleAPi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DriveGoogleAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriveVideoController : ControllerBase
    {
        private readonly GoogleDriveService _driveService;

        public DriveVideoController(GoogleDriveService driveService)
        {
            _driveService = driveService;
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadVideo(string fileId)
        {
            try
            {
                var stream = await _driveService.DownloadFileAsync(fileId);
                var fileName = await _driveService.GetFileNameAsync(fileId);

                // Using video/mp4 as default mime type, can be improved to detect dynamically
                return File(stream, "video/mp4", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }
    }
}