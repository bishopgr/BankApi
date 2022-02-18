using BankApi.Domain.Models;
using BankApi.Domain.Services;
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

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Should_Successfully_Deposit_Several_Amounts(decimal amount)
        {

            var userService = _server.Services.GetRequiredService<UserService>();
            var accountService = _server.Services.GetRequiredService<AccountService>();

            var createdUser = userService.CreateUser("testDude");

            accountService.Deposit(createdUser.Id, createdUser.Accounts[0].AccountId, amount);

            _server.Output?.WriteLine($"Deposited {amount} successfully. Balance is {createdUser.Accounts[0].Balance}");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void Should_Successfully_Withdraw_Several_Amounts(decimal amount)
        {

            var userService = _server.Services.GetRequiredService<UserService>();
            var accountService = _server.Services.GetRequiredService<AccountService>();

            var createdUser = userService.CreateUser("testDude");

            accountService.Withdraw(createdUser.Id, createdUser.Accounts[0].AccountId, amount);

            _server.Output?.WriteLine($"Withdrew {amount} successfully. Balance is {createdUser.Accounts[0].Balance}");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10001)]
        public void Should_Throw_On_Bad_Amount_Deposited(decimal amount)
        {

            var userService = _server.Services.GetRequiredService<UserService>();
            var accountService = _server.Services.GetRequiredService<AccountService>();

            var createdUser = userService.CreateUser("testDude");


            Assert.Throws<InvalidOperationException>(() => accountService.Withdraw(createdUser.Id, createdUser.Accounts[0].AccountId, amount));

            _server.Output?.WriteLine($"Threw exception from {nameof(accountService.Deposit)} for {amount} successfully.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(99)]
        public void Should_Throw_On_Bad_Amount_Withdrawn(decimal amount)
        {

            var userService = _server.Services.GetRequiredService<UserService>();
            var accountService = _server.Services.GetRequiredService<AccountService>();

            var createdUser = userService.CreateUser("testDude");


            Assert.Throws<InvalidOperationException>(() => accountService.Withdraw(createdUser.Id, createdUser.Accounts[0].AccountId, amount));

            _server.Output?.WriteLine($"Threw exception from {nameof(accountService.Withdraw)} for {amount} successfully.");
        }


    }
}
