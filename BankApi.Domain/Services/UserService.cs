using BankApi.Domain.Models;

namespace BankApi.Domain
{
    public class UserService
    {

        public User? GetUser(Guid userId)
        {
            var user = DataStore.Users.FirstOrDefault(u => u.Id == userId);

            if(user == null)
            {
                return null;
            }

            return user;
        }

        public User CreateUser(string userName)
        {
            var user = new User(userName);
            DataStore.Users.Add(user);

            return user;
        }

    }
}