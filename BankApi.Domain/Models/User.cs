using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Domain.Models
{
    //In a real environment there would be a lot more here like email address, additional contact info, etc.
    //This should probably be secured in some way such as encrypting the name and other sensitive info. 
    
    public record User
    {

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = "Unknown User";
        public List<Account>? Accounts { get; } = new List<Account>();

        public DateTime DateCreated { get; } = DateTime.UtcNow;

        //For simplicity, just give the user at least 100 FakeDollars when creating them.
        public User(string userName)
        {
            Name = userName;
            Accounts.Add(new Account(100));
        }
    }
}
