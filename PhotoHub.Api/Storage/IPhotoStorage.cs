
namespace PhotoHub.Api.Storage;

public record PhotoSaveResult(string RelativeUrl, string? AbsoluteUrl);

public interface IPhotoStorage
{
    Task<PhotoSaveResult> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
    Task<bool> DeleteAsync(string relativeUrlOrName, CancellationToken ct = default);
}
