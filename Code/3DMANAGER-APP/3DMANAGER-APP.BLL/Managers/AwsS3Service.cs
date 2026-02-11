using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.File;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<AwsS3Service> _logger;
        public AwsS3Service(IAmazonS3 s3Client, string bucketName, ILogger<AwsS3Service> logger)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
            _logger = logger;
        }
        public async Task DeleteImageAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        public async Task<FileResponse?> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder, int? groupId)
        {
            try
            {
                string key;
                if (groupId == null)
                {
                    key = $"{folder}/{Guid.NewGuid()}_{fileName}";
                }
                else
                {
                    key = $"group_{groupId}/{folder}/{Guid.NewGuid()}_{fileName}";
                }

                if (fileStream.CanSeek)
                    fileStream.Position = 0;

                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = fileStream,
                    ContentType = contentType
                };

                await _s3Client.PutObjectAsync(request);

                return new FileResponse
                {
                    FileKey = key,
                    FileUrl = $"https://{_bucketName}.s3.amazonaws.com/{key}"
                };
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "AWS ERROR");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AWS ERROR");
                return null;
            }
        }
        public string GetPresignedUrl(string key, int hours = 1)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(hours)
            };
            string responseURL = _s3Client.GetPreSignedURL(request);
            return responseURL;
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            string prefix = $"group_{groupId}/";

            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response listResponse;

            do
            {
                listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                if (listResponse.S3Objects.Count > 0)
                {
                    var deleteRequest = new DeleteObjectsRequest
                    {
                        BucketName = _bucketName,
                        Objects = listResponse.S3Objects
                            .Select(o => new KeyVersion { Key = o.Key })
                            .ToList()
                    };

                    await _s3Client.DeleteObjectsAsync(deleteRequest);
                }

                listRequest.ContinuationToken = listResponse.NextContinuationToken;

            } while (listResponse.IsTruncated == true);

        }

    }
}

