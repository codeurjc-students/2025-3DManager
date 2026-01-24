namespace _3DMANAGER_APP.BLL.Models.Base
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
