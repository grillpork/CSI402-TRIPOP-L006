using final_project.Models;

namespace final_project.Helpers;

public static class MediaHelper
{
    /// <summary>
    /// Saves an uploaded file into wwwroot/uploads/{mediaType}/ and records it in the media table.
    /// </summary>
    public static Media? SaveMedia(
        IFormFile? file,
        IWebHostEnvironment env,
        FinalCSI402DB db,
        string entityType = "General",
        int? entityId = null,
        int sortOrder = 0)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var contentType = file.ContentType ?? "application/octet-stream";
        var mediaType = DetectMediaType(contentType, file.FileName);
        var folderName = mediaType.ToLowerInvariant();

        var uploadsFolder = Path.Combine(env.WebRootPath, "uploads", folderName);
        Directory.CreateDirectory(uploadsFolder);

        var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var physicalPath = Path.Combine(uploadsFolder, storedFileName);

        using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var media = new Media
        {
            FileName = file.FileName,
            FilePath = $"/uploads/{folderName}/{storedFileName}",
            MediaType = mediaType,
            ContentType = contentType,
            EntityType = entityType,
            EntityId = entityId,
            SortOrder = sortOrder,
            UploadedAt = DateTime.Now
        };

        db.Media.Add(media);
        db.SaveChanges();

        return media;
    }

    private static string DetectMediaType(string contentType, string fileName)
    {
        if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return "Image";
        }

        if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
        {
            return "Video";
        }

        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        if (ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".svg")
        {
            return "Image";
        }

        if (ext is ".mp4" or ".webm" or ".mov" or ".avi" or ".mkv")
        {
            return "Video";
        }

        return "File";
    }
}
