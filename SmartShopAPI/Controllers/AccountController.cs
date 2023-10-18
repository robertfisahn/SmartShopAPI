using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Services;

namespace SmartShopAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("registration")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            var token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
