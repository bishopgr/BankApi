using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Domain.Models
{
    //This account class is trivial. In a real world, there would more properties here, possibly to help with frontend/fraud/security/etc 
    public record Account
    {
        public Guid AccountId { get; set; } = Guid.NewGuid();

        //Default to 100 for ez validation?
        public decimal Balance { get; set; }

        public Account(decimal balance)
        {
            Balance = ValidateBalance(balance);
        }

        private decimal ValidateBalance(decimal balance)
        {
            if(balance < 100)
            {
                throw new InvalidOperationException("The account balance cannot be below 100 FakeDollars.");
            }

            return balance;
        }

        public Account()
        {

        }
    }
}
