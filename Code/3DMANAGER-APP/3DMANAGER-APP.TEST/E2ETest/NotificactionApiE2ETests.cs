using _3DMANAGER_APP.BLL.Models.Notifications;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class NotificationsApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public NotificationsApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetUnreadNotifications_ShouldReturnNotificationsList()
        {
            var response = await _client.GetAsync("/api/v1/notifications/GetUnreadNotifications");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<NotificationObject>>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.Count >= 0);
        }
        [Fact]
        public async Task NotificationMarkAsRead_ShouldMarkNotificationSuccessfully()
        {

            var getResponse = await _client.GetAsync("/api/v1/notifications/GetUnreadNotifications");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getContent = await getResponse.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<NotificationObject>>>();

            Assert.NotNull(getContent);
            Assert.NotNull(getContent.Data);


            if (getContent.Data.Count == 0)
                return;

            var notificationId = getContent.Data.First().NotificationId;


            var markResponse = await _client.PostAsync($"/api/v1/notifications/NotificationMarkAsRead?notificationId={notificationId}", null);

            Assert.Equal(HttpStatusCode.OK, markResponse.StatusCode);

            var markContent = await markResponse.Content.ReadFromJsonAsync<Server.Models.CommonResponse<bool>>();

            Assert.NotNull(markContent);
            Assert.True(markContent.Data);
        }
    }
}