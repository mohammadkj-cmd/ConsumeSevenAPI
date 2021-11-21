using System;
using System.Text.Json.Serialization;

namespace ProcessSevenAPI.Model
{
    public class Person
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first")]
        public string FirstName { get; set; }

        [JsonPropertyName("last")]
        public string LastName { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }
}