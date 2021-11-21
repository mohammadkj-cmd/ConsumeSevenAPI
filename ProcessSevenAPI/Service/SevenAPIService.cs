using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ProcessSevenAPI.Model;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ProcessSevenAPI.Service
{
    public class SevenAPIService : ISevenAPIService
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ILogger<SevenAPIService> _logger;
        private readonly IConfiguration _configuration;

        private List<Person> persons;

        public SevenAPIService(HttpClient httpClient, ILogger<SevenAPIService> logger, IConfiguration config)
        {
            _client = httpClient;
            _logger = logger;
            _configuration = config;
        }

        public async Task<string> GetPersonFullName(int Id = 42)
        {
            try
            {
                var streamTask = await _client.GetStreamAsync(_configuration.GetSection("URL").Value);
                this.persons = await JsonSerializer.DeserializeAsync<List<Person>>(streamTask);

                var person = persons.Find(item => item.Id == Id);
                if (person != null)
                {
                    return person.FirstName + " " + person.LastName;
                }
                else
                {
                    return $"Person with ID #{Id} not found";
                }
            }catch (Exception ex)
            {
                // Logging complete information with stack trace
                _logger.LogError(ex.ToString());
                // Returning only error message
                return "Error: " + ex.Message;
            }
        }

        public async Task<string> GetAllFirstNames(int Age = 23)
        {
            try
            {
                var streamTask = await _client.GetStreamAsync(_configuration.GetSection("URL").Value);
                this.persons = await JsonSerializer.DeserializeAsync<List<Person>>(streamTask);

                var listPerson = persons.FindAll(item => item.Age == Age);
                if (listPerson != null && listPerson.Count > 0)
                {
                    return string.Join(", ", listPerson.Select(p => p.FirstName));
                }
                else
                {
                    return $"Person with Age {Age} not found";
                }
            }
            catch (Exception ex)
            {
                // Logging complete information with stack trace
                _logger.LogError(ex.ToString());
                // Returning only error message
                return "Error: " + ex.Message;
            }

        }

        public async Task<string> GetGenderInfo()
        {
            try 
            {
                var streamTask = await _client.GetStreamAsync(_configuration.GetSection("URL").Value);
                this.persons = await JsonSerializer.DeserializeAsync<List<Person>>(streamTask);

                var personGroup = persons.GroupBy(p => new { p.Age, p.Gender })
                    .OrderBy(x => x.Key.Age)
                    .Select(item => new { Key = item.Key, Count = item.Select(i => i.Gender).Count() });

                if (personGroup != null && personGroup.Count() > 0)
                {
                    string genderInfo = string.Empty;
                    foreach (var age in personGroup.Select(item => item.Key.Age).Distinct().ToList())
                    {
                        var ageGroup = personGroup.Where(item => item.Key.Age == age).ToList();
                        if (ageGroup.Count() > 0)
                        {
                            string record = string.Empty;
                            string ageString = $"Age: {age}";
                            foreach (var aGroup in ageGroup)
                            {
                                string gender = aGroup.Key.Gender.Replace("M", "Male").Replace("F", "Female");
                                record += $" {gender}: {aGroup.Count } ";
                            }
                            genderInfo += ageString + record + "\n";
                        }
                    }

                    return genderInfo;
                }
                else
                {
                    return $"No grouping found";
                }
            }
            catch (Exception ex)
            {
                // Logging complete information with stack trace
                _logger.LogError(ex.ToString());
                // Returning only error message
                return "Error: " + ex.Message;
            }
        }
    }
}
