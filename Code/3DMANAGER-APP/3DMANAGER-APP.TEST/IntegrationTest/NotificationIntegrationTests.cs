using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Repositories;
using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class NotificationIntegrationTests
    {

        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserRepository _userRepository;

        public NotificationIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();

            var emailMock = new Mock<IEmailService>();
            emailMock
                .Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                ))
                .Returns(Task.CompletedTask);

            _emailService = emailMock.Object;

            _userRepositoryMock = new Mock<IUserRepository>();

            _userRepository = _userRepositoryMock.Object;
        }



        [Fact]
        public void GetUnreadNotifications_ShouldReturnList()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var repository = new NotificationRepository(
                dataSource,
                NullLogger<NotificationRepository>.Instance
            );



            var service = new NotificationService(
                repository,
                _mapper,
                NullLogger<NotificationService>.Instance,
                _emailService,
                _userRepository
            );

            BaseError? error;
            var result = service.GetUnreadNotifications(1, out error);

            Assert.Null(error);
            Assert.NotNull(result);
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public void NotificationMarkAsRead_ShouldMarkCorrectly()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var repository = new NotificationRepository(
                dataSource,
                NullLogger<NotificationRepository>.Instance
            );

            var fakeEmail = new FakeEmailService();

            var service = new NotificationService(
                repository,
                _mapper,
                NullLogger<NotificationService>.Instance,
                _emailService,
                _userRepository
            );

            BaseError? error;
            var notifications = service.GetUnreadNotifications(1, out error);

            if (notifications.Count == 0)
                return;

            var id = notifications.First().NotificationId;

            var result = service.NotificationMarkAsRead(id, out error);

            Assert.True(result);
            Assert.Null(error);
        }
    }


}
