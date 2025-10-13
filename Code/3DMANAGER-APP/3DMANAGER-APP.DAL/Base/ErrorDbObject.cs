namespace _3DMANAGER_APP.DAL.Base
{
    public class ErrorDbObject
    {
        public int code { get; set; }
        public string message { get; set; }
        public ErrorDbObject()
        {
        }
        public ErrorDbObject(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
