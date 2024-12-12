using _1likteEcommerce.Api.Models;
using _1likteEcommerce.Core.Models;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService, IFileService fileService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username, Email = model.Email, UserBasket = new Basket() {BasketItems = new List<BasketItem>(), CreatedAt = DateTime.Now } };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("User Created");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
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
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
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

        [Authorize]
        [HttpPost("{userId}/upload-photo")]
        public async Task<IActionResult> UploadProfilePhoto(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var extension = Path.GetExtension(file.FileName);
            if(extension != ".png" && extension != ".jpg" && extension != ".jpeg") return BadRequest("Invalid file type");

            await using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            var uniqueFileName = $"{userId}_{extension}";
            await _fileService.UploadFileAsync(bytes, uniqueFileName);

            user.PhotoPath = uniqueFileName;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}/download-photo")]
        public async Task<IActionResult> DownloadProfilePhoto(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            if (string.IsNullOrEmpty(user.PhotoPath)) return BadRequest("User photo not found");

            var bytes = await _fileService.GetFileBytesAsync(user.PhotoPath);
            if(bytes == null || bytes.Length == 0) BadRequest("User photo not found");

            string contentType = Path.GetExtension(user.PhotoPath) switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
            var fileName = Path.GetFileName(user.PhotoPath);
            return File(bytes, contentType, fileName);
        }
    }
}
