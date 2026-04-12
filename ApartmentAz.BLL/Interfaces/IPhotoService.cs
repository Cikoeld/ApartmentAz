using Microsoft.AspNetCore.Http;

namespace ApartmentAz.BLL.Interfaces;

public interface IPhotoService
{
    Task<string> SaveImageAsync(IFormFile file, string folder);
    Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folder);
    void DeleteImage(string relativePath);
}
