using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.DAL.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MySql.Data.MySqlClient;

namespace _3DMANAGER_APP.TEST
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("CI");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Server=localhost;Port=3307;Database=3dmanagerCI;User=root;Password=password;"
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IDataSource<MySqlConnection>));

                services.AddScoped<IDataSource<MySqlConnection>>(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var cs = configuration.GetConnectionString("DefaultConnection");

                    return new MySQLDataSource(cs!, "3DMANAGER");
                });

            });
            builder.ConfigureServices(services =>
            {
                var s3Mock = new Mock<IAwsS3Service>();
                s3Mock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>())).Returns("https://fake-url.com/presigned/test.jpg");
                services.AddSingleton<IAwsS3Service>(s3Mock.Object);
            });
        }
    }
}
