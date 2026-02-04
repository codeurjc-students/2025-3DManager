using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class UserIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _fakeS3Service;
        public UserIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeS3Service = new FakeAwsS3Service();
        }

        [Fact]
        public void User_ShouldReturnUser()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var userDbManager = new UserDbManager(
                dataSource,
                NullLogger<UserDbManager>.Instance
            );

            var manager = new UserManager(
                userDbManager,
                _mapper,
                NullLogger<UserManager>.Instance,
                _fakeS3Service
            );

            BaseError error;
            var users = manager.GetUserList(1, out error);

            Assert.Null(error);
            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }
    }
}
