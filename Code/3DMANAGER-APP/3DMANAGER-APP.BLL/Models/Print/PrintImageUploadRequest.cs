using Microsoft.AspNetCore.Http;

namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintImageUploadRequest
    {
        public IFormFile ImageFile { get; set; }
    }

}
