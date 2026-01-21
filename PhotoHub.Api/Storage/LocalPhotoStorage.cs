
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace PhotoHub.Api.Storage;

public sealed class LocalPhotoStorage : IPhotoStorage
{
    private readonly IWebHostEnvironment _env;

    public LocalPhotoStorage(IWebHostEnvironment env) => _env = env;

    public async Task<PhotoSaveResult> SaveAsync(
        Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        var webRoot = _env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
        var uploadsRoot = Path.Combine(webRoot, "uploads");
        Directory.CreateDirectory(uploadsRoot);

        var ext = Path.GetExtension(fileName);
        var safeBase = Sanitize(Path.GetFileNameWithoutExtension(fileName));
        var uniqueName = $"{safeBase}_{Guid.NewGuid():N}{ext}";
        var physicalPath = Path.Combine(uploadsRoot, uniqueName);

        using (var fs = new FileStream(physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            await fileStream.CopyToAsync(fs, ct);
        }

        // Relative URL works with whatever host the API is on (Codespaces/localhost)
        var relativeUrl = $"/uploads/{uniqueName}";
        return new PhotoSaveResult(relativeUrl, null);
    }

    public Task<bool> DeleteAsync(string relativeUrlOrName, CancellationToken ct = default)
    {
        try
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
            string relative = relativeUrlOrName.StartsWith("/") ? relativeUrlOrName[1..] : relativeUrlOrName;

            // If a full URL is passed, try to extract the file name after /uploads/
            var idx = relative.IndexOf("uploads/", StringComparison.OrdinalIgnoreCase);
            if (idx >= 0) relative = relative[(idx + "uploads/".Length)..];

            var physicalPath = Path.Combine(webRoot, "uploads", relative);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "file";
        var invalid = Path.GetInvalidFileNameChars().ToHashSet();
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
            sb.Append(invalid.Contains(ch) ? '_' : ch);
        return sb.ToString();
    }
}
