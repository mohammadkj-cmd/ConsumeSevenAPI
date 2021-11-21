using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSevenAPI.Service
{
    public interface ISevenAPIService
    {
        Task<string> GetPersonFullName(int Id = 42);
        Task<string> GetAllFirstNames(int Age = 23);
        Task<string> GetGenderInfo();
    }
}
