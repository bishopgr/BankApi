using BankApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        //This can be a bug if the userId and accountId are transposed. Turn into a strongly typed ID to fix this issue.
        [HttpPost("deposit")]

        //To simplify this you can make a DepositRequest record or something similar anbd pass that in instead if this were coming from a frontend.
        public IActionResult Deposit([FromBody] Transaction transaction)
        {
            _accountService.Deposit(transaction.UserId, transaction.AccountId, transaction.Amount);

            return CreatedAtAction(nameof(Deposit), $"Deposited {transaction.Amount} into account {transaction.AccountId}");
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] Transaction transaction)
        {
            _accountService.Withdraw(transaction.UserId, transaction.AccountId, transaction.Amount);

            //Maybe show remaining balance?
            return CreatedAtAction(nameof(Withdraw), $"Withdrew {transaction.Amount} from account {transaction.AccountId}");
        }

        [HttpDelete("close")]
        public IActionResult DeleteAccount([FromBody] AccountRemovalRequest removalRequest)
        {
            _accountService.DeleteAccount(removalRequest.UserId, removalRequest.AccountId);

            return NoContent();
        }

        [HttpPost("create")]
        public IActionResult CreateAccount([FromBody] AccountCreationRequest accountCreationRequest)
        {
            _accountService.CreateAccount(accountCreationRequest.UserId, accountCreationRequest.DepositAmount);

            return CreatedAtAction(nameof(CreateAccount), $"Created account successfully for user {accountCreationRequest.UserId}");
        }

        //Can use a base class to fix this up
        public record Transaction
        {
            public Guid UserId { get; set; }
            public Guid AccountId { get; set; }

            public decimal Amount { get; set; }
        }

        public record AccountRemovalRequest
        {
            public Guid UserId { get; set; }

            public Guid AccountId { get; set; }
        }

        public record AccountCreationRequest
        {
            public Guid UserId { get; set; }
            public decimal DepositAmount { get; set; }
        }
    }
}
