using _1likteEcommerce.Api.Models;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1likteEcommerce.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserService _userService;
        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("User Created");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user == null) return Unauthorized();

                    var token = _userService.GenerateJwtToken(user);
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound("User not found");

                user.Email = model.Email ?? user.Email;
                user.UserName = model.Username ?? user.UserName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok("User updated");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}
