using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ApartmentAz.BLL.Services;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _env;

    public PhotoService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(IFormFile file, string folder)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", folder);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/images/{folder}/{uniqueName}";
    }

    public async Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folder)
    {
        var urls = new List<string>();

        foreach (var file in files)
        {
            var url = await SaveImageAsync(file, folder);
            urls.Add(url);
        }

        return urls;
    }

    public void DeleteImage(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return;

        var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
