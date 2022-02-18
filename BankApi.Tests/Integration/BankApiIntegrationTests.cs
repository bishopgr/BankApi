using BankApi.Domain;
using BankApi.Domain.Models;
using BankApi.Tests.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BankApi.Tests.Integration
{
    public class BankApiIntegrationTests
    {
        private readonly TestServerBuilder _server;
       
        public BankApiIntegrationTests(ITestOutputHelper output)
        {
            _server = new TestServerBuilder(output);
        }

        [Fact]
        public void Should_Create_User_With_Valid_Account()
        {
            var userService = _server.Services.GetRequiredService<UserService>();

            var createdUser = userService.CreateUser("testDude");
            var userId = createdUser.Id;
            var accountId = createdUser?.Accounts?[0].AccountId;

            Assert.NotNull(createdUser);
            Assert.Equal("testDude", createdUser?.Name);
            Assert.Equal(userId, createdUser?.Id);


            Assert.All(createdUser?.Accounts, (element) =>
            {
                Assert.Equal(accountId, element.AccountId);
                Assert.Equal(100, element.Balance);
            });
            _server?.Output?.WriteLine($"{createdUser}{Environment.NewLine}{createdUser?.Accounts?[0]}");
            Assert.True(true);
        }
    }
}
