using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class FakeAzureBlobStorageService : IAzureBlobStorageService
    {
        public Task DeleteGroupAsync(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteImageAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string GetPresignedUrl(string key, int hours = 1)
        {
            throw new NotImplementedException();
        }

        public Task<FileResponse?> UploadImageAsync(Stream stream, string fileName, string contentType, string folder)
        {
            return Task.FromResult<FileResponse?>(new FileResponse
            {
                FileKey = "folder/abs_storage_3dmanager_key",
                FileUrl = $"https://fake-abs/{folder}/{fileName}"
            });
        }

        public Task<FileResponse?> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder, int? groupId)
        {
            throw new NotImplementedException();
        }
    }

}
