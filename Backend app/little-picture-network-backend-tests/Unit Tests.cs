using LittlePictureNetworkBackend;
using LittlePictureNetworkBackend.Data;
using LittlePictureNetworkBackend.DTOs;
using LittlePictureNetworkBackend.Models;
using LittlePictureNetworkBackend.PhotoConvertors;
using LittlePictureNetworkBackend.VirusScanners;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Testcontainers.MsSql;

namespace LittlePictureNetworkBackend_tests;

public class IntegrationTests_AlbumController : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    internal readonly MsSqlContainer _msSqlContainer;

    public sealed class IntegrationTestWebAppFactory<TDbContext> : WebApplicationFactory<Program> where TDbContext : DbContext
    {
        private readonly string _connectionString;

        public IntegrationTestWebAppFactory(IntegrationTests_AlbumController controllerFixture)
        {
            _connectionString = controllerFixture._msSqlContainer.GetConnectionString();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<TDbContext>) == service.ServiceType));
                services.Remove(services.SingleOrDefault(service => typeof(DbConnection) == service.ServiceType));
                services.AddDbContext<TDbContext>((_, option) => option.UseSqlServer(_connectionString));
            });
        }
    }

    public IntegrationTests_AlbumController(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _msSqlContainer = new MsSqlBuilder().Build();
    }

    public Task InitializeAsync()
    {
        return _msSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _msSqlContainer.DisposeAsync().AsTask();
    }

    public sealed class IndexPageTests : IClassFixture<IntegrationTests_AlbumController>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        private readonly HttpClient _httpClient;

        public IndexPageTests(IntegrationTests_AlbumController fixture)
        {
            var clientOptions = new WebApplicationFactoryClientOptions();
            clientOptions.AllowAutoRedirect = false;

            _webApplicationFactory = new IntegrationTestWebAppFactory<PictureNetworkDbContext>(fixture);
            _httpClient = _webApplicationFactory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            _webApplicationFactory.Dispose();
        }
    }



    [Fact]
    public async Task CreateAlbum_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var albumDto = new AlbumDto { UserId = "user-id", Title = "Test Album" };
        var content = new StringContent(JsonConvert.SerializeObject(albumDto), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/album/create", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // You can add additional assertions based on your requirements
    }

    [Fact]
    public async Task CreateAlbum_WithInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var invalidAlbumDto = new AlbumDto(); // Invalid input
        var content = new StringContent(JsonConvert.SerializeObject(invalidAlbumDto), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/album/create", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // You can add additional assertions based on your requirements
    }

    public void Dispose()
    {
        // Clean up resources after tests are executed
        _msSqlContainer?.DisposeAsync();
    }
}

public class UitTests
{

    [Fact]
    public void CreateToken_ReturnsValidToken()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            Login = "testuser",
            Role = "user"
        };

        // Act
        var token = JwtHandler.CreateToken(user);

        // Assert
        Assert.NotNull(token);

        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token);

        // Check claims
        Assert.Contains(decodedToken.Claims, c => c.Type == "id" && c.Value == "00000000-0000-0000-0000-000000000000");
        Assert.Contains(decodedToken.Claims, c => c.Type == "login" && c.Value == "testuser");
        Assert.Contains(decodedToken.Claims, c => c.Type == "role" && c.Value == "user");

        // Check other token properties
        Assert.Equal("https://localhost:7245", decodedToken.Issuer);
        Assert.Equal("https://localhost:7245", decodedToken.Audiences.ToArray()[0]);
        Assert.True(decodedToken.ValidFrom < DateTime.UtcNow);
        Assert.True(decodedToken.ValidTo > DateTime.UtcNow);
    }

    [Fact]
    public void ToByteArray_ConvertsBase64StringToByteArray()
    {
        // Arrange
        var converter = new SimplePhotoConverter();
        var base64String = "137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,6,0,0,0,6,8,6,0,0,0,224,204,239,72,0,0,0,9,112,72,89,115,0,0,11,19,0,0,11,19,1,0,154,156,24,0,0,0,49,73,68,65,84,8,153,99,100,96,96,248,255,60,141,1,5,72,206,98,96,96,124,158,198,240,159,1,11,96,66,86,37,57,11,139,4,58,96,129,49,208,237,97,66,214,142,108,44,0,135,106,9,113,161,219,170,118,0,0,0,0,73,69,78,68,174,66,96,130";
        var expectedByteArr = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 6, 0, 0, 0, 6, 8, 6, 0, 0, 0, 224, 204, 239, 72, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 11, 19, 0, 0, 11, 19, 1, 0, 154, 156, 24, 0, 0, 0, 49, 73, 68, 65, 84, 8, 153, 99, 100, 96, 96, 248, 255, 60, 141, 1, 5, 72, 206, 98, 96, 96, 124, 158, 198, 240, 159, 1, 11, 96, 66, 86, 37, 57, 11, 139, 4, 58, 96, 129, 49, 208, 237, 97, 66, 214, 142, 108, 44, 0, 135, 106, 9, 113, 161, 219, 170, 118, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };

        // Act
        var result = converter.ToByteArray(base64String);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedByteArr, result);
    }


    private readonly string _testPngImagePath = "../../../photo.png";
    [Fact]
    public void ToJpeg_ConvertsByteArrayToJpegImage()
    {
        // Arrange
        var converter = new SimplePhotoConverter();
        var imageBytes = File.ReadAllBytes(_testPngImagePath);

        // Act
        var result = converter.ToJpeg(imageBytes);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);

        // Additional assertions if needed
        // For example, you might want to check the format of the converted image.
        using var ms = new MemoryStream(result);
        var convertedImage = Image.FromStream(ms);
        Assert.Equal(ImageFormat.Jpeg, convertedImage.RawFormat);
    }

    [Fact]
    public void ToJpeg_ThrowsExceptionOnInvalidByteArray()
    {
        // Arrange
        var converter = new SimplePhotoConverter();
        var invalidByteArray = new byte[0]; // Invalid byte array

        // Act & Assert
        Assert.Throws<ArgumentException>(() => converter.ToJpeg(invalidByteArray));
    }

    [Fact]
    public void ToJpeg_ReturnsSameByteArrayForJpegImage()
    {
        // Arrange
        var converter = new SimplePhotoConverter();
        var jpegImageBytes = File.ReadAllBytes(_testPngImagePath);

        // Act
        var result = converter.ToJpeg(jpegImageBytes);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);

        // Additional assertion: Check that the result is the same as the input for JPEG images
        Assert.Equal(jpegImageBytes, result);
    }

    [Fact]
    public void ToJpeg_ReturnsDifferentByteArrayForNonJpegImage()
    {
        // Arrange
        var converter = new SimplePhotoConverter();
        var pngImageBytes = File.ReadAllBytes(_testPngImagePath);

        // Act
        var result = converter.ToJpeg(pngImageBytes);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);

        // Additional assertion: Check that the result is different from the input for non-JPEG images
        Assert.NotEqual(pngImageBytes, result);
    }

    [Fact]
    public void ScanPhotoForViruses_ReturnsTrueWhenNoVirusesFound()
    {
        // Arrange
        var scanner = new WindowsEmbededVirusScanner();
        var photoData = File.ReadAllBytes(_testPngImagePath);

        // Act
        var result = scanner.ScanPhotoForViruses(photoData);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ScanPhotoForViruses_ReturnsFalseWhenVirusesFound()
    {
        // Arrange
        var scanner = new WindowsEmbededVirusScanner();
        // Create a test file containing virus or use a mock object for the scanner
        var infectedPhotoData = File.ReadAllBytes("path/to/your/infected/test/image.jpg");

        // Act
        var result = scanner.ScanPhotoForViruses(infectedPhotoData);

        // Assert
        Assert.False(result);
    }
}