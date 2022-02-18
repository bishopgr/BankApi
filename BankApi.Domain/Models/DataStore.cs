using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Domain.Models
{
    public static class DataStore
    {
        public static List<User> Users { get; set; } = new List<User>();
    }
}
