using System.Text.Json.Serialization;

namespace _3DMANAGER.API.Models
{
    public class Response
    {
        [JsonPropertyName("error")]
        public ErrorProperties? Error { get; set; }
        public Response()
        {
        }
        public Response(ErrorProperties Error)
        {
            this.Error = Error;
        }
        public class ErrorProperties
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
            [JsonIgnore]
            public int? StatusCode { get; set; }
            public ErrorProperties(int Code,string Message)
            {
                this.Code = Code;
                this.Message = Message;
            }
            public ErrorProperties(int Code, string Message, int StatusCode)
            {
                this.Code = Code;
                this.Message = Message;
                this.StatusCode = StatusCode;
            }
            public ErrorProperties()
            {

            }
        }
    }
}
