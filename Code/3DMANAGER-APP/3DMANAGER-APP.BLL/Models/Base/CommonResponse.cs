using System.Text.Json.Serialization;

namespace _3DMANAGER_APP.BLL.Models.Base
{
    public class CommonResponse<Model> : Response
    {
        [JsonPropertyName("data")]
        public Model? Data { get; set; }
        public CommonResponse(Model Data)
        {
            this.Data = Data;
        }

        public CommonResponse(ErrorProperties Error) : base(Error)
        {
        }
        public CommonResponse()
        {
        }
    }
}
