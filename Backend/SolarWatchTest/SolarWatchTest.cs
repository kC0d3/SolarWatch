/*using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Services;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SolarDataProvider;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarDataProvider> _solarDataProviderMock;
    private Mock<ICityDataProvider> _cityDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ISolarRepository> _solarRepository;
    private SolarWatchController _watchController;

    /*[SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _solarDataProviderMock = new Mock<ISolarDataProvider>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _solarRepository = new Mock<ISolarRepository>();
        _controller = new SolarWatchController(_loggerMock.Object, _cityDataProviderMock.Object,
            _solarDataProviderMock.Object,
            _jsonProcessorMock.Object, _cityRepositoryMock.Object, _solarRepository.Object);
    }

    /*[Test]
    public void GetSolarDataReturnsNotFoundResultIfSolarDataProviderFails()
    {
        // Arrange
        var solarData = "{}";
        _solarDataProviderMock.Setup(x => x.GetSolarData(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Throws(new Exception());

        // Act
        var result = _controller.GetSolarData("Budapest", DateTime.Today);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public void GetSolarDataReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _solarDataProviderMock.Setup(x => x.GetSolarData(It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarDataFromAPI(solarData)).Throws<Exception>();

        // Act
        var result = _controller.GetSolarData("Budapest", DateTime.Now);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public void GetSolarDataReturnsOkResultIfSolarDataIsValid()
    {
        // Arrange
        var expectedSolarData = new Solar();
        var solarData = "{}";
        _solarDataProviderMock.Setup(x => x.GetSolarData(It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarData(solarData)).Returns(expectedSolarData);

        // Act
        var result = _controller.GetSolarData("Budapest", DateTime.Today);

        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedSolarData));
    }
}*/

