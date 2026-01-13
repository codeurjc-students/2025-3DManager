namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IAwsS3Service
    {
        public Task<string> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder);
        public Task DeleteImageAsync(string key);
    }
}
