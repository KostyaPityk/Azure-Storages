using System.Threading.Tasks;
using Bloor.Service;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Bloor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloorController : ControllerBase
    {
        private readonly ImageService _imageService;
        private readonly Logger _logger;

        public BloorController(ImageService imageService)
        {
            _imageService = imageService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(string id, string saveDirectory)
        {
            _logger.Info($"Loading image by id.");
            await _imageService.DowmloadImageAsync(id, saveDirectory);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> PostImage(string id, string filePath)
        {
            _logger.Info($"Upload image by id.");
            await _imageService.UploadImageAsync(id, filePath);
            return Ok();
        }
    }
}
