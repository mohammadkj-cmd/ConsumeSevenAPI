using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcessSevenAPI.Service;
using System;
using System.Threading.Tasks;

namespace ConsumeSevenAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Service URL is in configuration file
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Service registration using .net core DI
            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddSingleton<ISevenAPIService, SevenAPIService>()
            .AddSingleton(config)
            .BuildServiceProvider();

            // Service Instance
            var processAPI = serviceProvider.GetService<ISevenAPIService>();

            // Service Calling
            Console.WriteLine(await processAPI.GetPersonFullName(31));
            Console.WriteLine(await processAPI.GetAllFirstNames());
            Console.WriteLine(await processAPI.GetGenderInfo());
        }
    }
}
