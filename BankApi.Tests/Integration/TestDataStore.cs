using BankApi.Domain.Models;
using System.Collections.Generic;

namespace BankApi.Tests.Integration
{
    public static class TestDataStore
    {
        public static List<User>? Users { get; set; } = new List<User>();
    }
}