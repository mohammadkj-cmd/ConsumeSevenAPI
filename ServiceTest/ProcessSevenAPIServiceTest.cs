using ProcessSevenAPI.Service;
using System;
using Xunit;
using Moq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ServiceTest
{
    public class ProcessSevenAPIServiceTest
    {
        private SevenAPIService _sevenAPIService;

        public ProcessSevenAPIServiceTest()
        {
            // Moqing httpclient and logger object
            var mockHttpClient = new Mock<HttpClient>();
            var mockLogger = new Mock<ILogger<SevenAPIService>>();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _sevenAPIService = new SevenAPIService(mockHttpClient.Object, mockLogger.Object, configuration);
        }

        [Fact]
        public async void ProcessSevenAPIServiceTest_GetPersonFullName_ShouldPass()
        {
            var r = await _sevenAPIService.GetPersonFullName(31);
            Assert.True(r.Equals("Jill Scott"));
        }

        [Fact]
        public async void ProcessSevenAPIServiceTest_GetAllFirstNames_ShouldPass()
        {
            var r = await _sevenAPIService.GetAllFirstNames();
            Assert.True(!r.Contains("Error"));
        }

        [Fact]
        public async void ProcessSevenAPIServiceTest_GetGenderInfo_ShouldPass()
        {
            var r = await _sevenAPIService.GetGenderInfo();
            Assert.Contains("Age", r);
        }
    }
}
