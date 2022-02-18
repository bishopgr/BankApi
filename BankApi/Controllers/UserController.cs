using BankApi.Domain.Services;
using BankApi.Domain.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] string name)
        {
            var createdUser = _userService.CreateUser(name);

            if(createdUser != null)
            {
                return CreatedAtAction(nameof(CreateUser), createdUser);
            }

            return BadRequest("The user could not be created.");
        }

        //Cheating to see what users we have in the db
        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            return Ok(DataStore.Users);
        }

    }
}
