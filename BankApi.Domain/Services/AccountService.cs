using BankApi.Domain.Models;
using BankApi.Domain.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace BankApi.Domain.Services
{
    //The account service is trivial.
    public class AccountService
    {

        private readonly IMemoryCache _cache;

        public AccountService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public void CreateAccount(Guid userId, decimal downDepositAmount)
        {
            var user = GetUser(userId);

            user?.Accounts?.Add(new Account(downDepositAmount));
        }

        public void DeleteAccount(Guid userId, Guid accountId)
        {
            var user = GetUser(userId);
            var accountToBeRemoved = GetAccountForUser(user!, accountId);

            try
            {
                user?.Accounts?.Remove(accountToBeRemoved);
            }
            catch
            {
                throw;
            }
        }


        //Primitive types. Be careful with the guid order.
        public void Deposit(Guid userId, Guid accountId, decimal depositAmount)
        {
            var user = GetUser(userId);

            CheckValidDeposit(depositAmount);

            var account = user?.Accounts?.FirstOrDefault(a => a.AccountId == accountId);

            account!.Balance += depositAmount;

        }

        private static void CheckValidDeposit(decimal depositAmount)
        {
            if (depositAmount < decimal.Zero)
            {
                throw new InvalidOperationException("You cannot deposit a negative amount.");
            }

            if (depositAmount == decimal.Zero)
            {
                throw new InvalidOperationException("You cannot deposit 0 FakeDollars into this account.");
            }

            if (depositAmount > 10000)
            {
                throw new InvalidOperationException("You cannot deposit more than 10000 FakeDollars at once into an account.");
            }
        }

        public void Withdraw(Guid userId, Guid accountId, decimal withdrawAmount)
        {
            var user = GetUser(userId);

            var account = user?.Accounts?.FirstOrDefault(a => a.AccountId == accountId);

            CheckValidWithdraw(account!.Balance!, withdrawAmount);


            account.Balance -= withdrawAmount;
        }

        private static void CheckValidWithdraw(decimal balance, decimal amount)
        {

            if (balance < amount)
            {
                throw new InvalidOperationException($"You cannot withdraw more than what your current balance is.");
            }

            if (balance < 100)
            {
                throw new InvalidOperationException("You cannot have an account balance below 100 FakeDollars.");
            }



            if (amount < decimal.Zero || amount == decimal.Zero)
            {
                throw new InvalidOperationException($"The withdrawal amount is not a valid value. Amount entered {amount} ");
            }

            CheckIfWithdrawingTooMuch(balance, amount);


            static bool CheckIfWithdrawingTooMuch(decimal balance, decimal amount)
            {
                var isWithDrawingTooMuch = amount > balance / 100 * 90;

                if (isWithDrawingTooMuch)
                {
                    throw new InvalidOperationException($"The withdrawal amount cannot exceed 90% of your balance.");
                }

                return isWithDrawingTooMuch;
            }
        }


        private User? GetUser(Guid userId)
        {
            var cachedUser = _cache.Get<User>(userId);

            if (cachedUser == null)
            {
                var user = DataStore.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null) { throw new InvalidOperationException($"The user id {userId} was not found."); }

                _cache.Set(userId, user);

                return user;
            }


            return cachedUser;
        }

        private Account GetAccountForUser(User user, Guid accountId)
        {
            return user.Accounts.Where(a => a.AccountId == accountId).FirstOrDefault();
        }

    }
}
