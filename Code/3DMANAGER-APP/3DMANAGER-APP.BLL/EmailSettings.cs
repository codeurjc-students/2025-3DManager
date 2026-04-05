namespace _3DMANAGER_APP.BLL
{
    public class EmailSettings
    {
        public string From { get; set; } = string.Empty;
        public string Smtp { get; set; } = string.Empty;
        public int Port { get; set; }
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
