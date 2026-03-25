using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IAzureBlobStorageService
    {
        public Task<FileResponse?> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder, int? groupId);
        public Task DeleteImageAsync(string key);
        public Task DeleteGroupAsync(int groupId);
        string GetPresignedUrl(string key, int hours = 1);
    }

}
