using System.Text.Json.Serialization;

namespace _3DMANAGER_APP.Server.Models
{
    public class CommonResponse<Model> : Response
    {
        [JsonPropertyName("data")]
        public Model? Data { get; set; }
        public CommonResponse(Model Data)
        {
            this.Data = Data;
        }

        public CommonResponse(ErrorProperties Error): base(Error)
        {
        }
        public CommonResponse()
        {
        }
    }
}
