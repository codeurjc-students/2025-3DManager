using _3DMANAGER_APP.BLL.Interfaces;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class FakeEmailService : IEmailService
    {
        public List<(string To, string Subject, string Body, bool IsHtml)> SentEmails { get; } = new();

        public Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            SentEmails.Add((to, subject, body, isHtml));
            return Task.CompletedTask;
        }
    }

}
