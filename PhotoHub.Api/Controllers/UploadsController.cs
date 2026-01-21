
using Microsoft.AspNetCore.Mvc;
using PhotoHub.Api.Storage;

namespace PhotoHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadsController : ControllerBase
{
    private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/webp", "image/gif" };
    private const long MaxBytes = 20 * 1024 * 1024; // 20 MB

    private readonly IPhotoStorage _storage;

    public UploadsController(IPhotoStorage storage) => _storage = storage;

    [HttpPost]
    [RequestSizeLimit(MaxBytes)]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (file.Length > MaxBytes)
            return BadRequest("File too large (max 20 MB)");

        if (!AllowedImageTypes.Contains(file.ContentType))
            return BadRequest($"Unsupported file type: {file.ContentType}");

        await using var stream = file.OpenReadStream();
        var saved = await _storage.SaveAsync(stream, file.FileName, file.ContentType, ct);

        // If you want to persist metadata in your DB, do it here:
        // var photo = new Photo { Title = Path.GetFileNameWithoutExtension(file.FileName), Url = saved.RelativeUrl };
        // _db.Photos.Add(photo); await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            url = saved.RelativeUrl,       // e.g., /uploads/abc123.jpg
            contentType = file.ContentType,
            size = file.Length
        });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string url, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(url)) return BadRequest("Missing url");
        var ok = await _storage.DeleteAsync(url, ct);
        return ok ? NoContent() : NotFound();
    }
}
 
