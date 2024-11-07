using Microsoft.AspNetCore.Mvc;
using Hotaku.Persistence.Entities;
using Hotaku.Main.Services;

namespace Hotaku.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public Task<List<User>> GetAllUsers()
        {
            return userService.GetAllUsers();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await userService.GetUserById(userId);

                return Ok(user);
            }
            catch (Exception ex) {
                return NotFound(new { message = ex.Message });
            }

        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var message = await userService.DeleteUser(userId);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                user.CreatedAt = DateTime.Now;
                var newUser = await userService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}