namespace _3DMANAGER.API.Models
{
    public class BaseError
    {
        public int code { get; set; }
        public string message { get; set; }
        public BaseError()
        {
        }
        public BaseError(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
