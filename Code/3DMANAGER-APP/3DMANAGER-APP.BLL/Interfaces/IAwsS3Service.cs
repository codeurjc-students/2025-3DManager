using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IAwsS3Service
    {
        Task<FileResponse?> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder, int? groupId);
        Task DeleteImageAsync(string key);
        Task DeleteGroupAsync(int groupId);
        public string GetPresignedUrl(string key, int hours = 1);
    }
}
