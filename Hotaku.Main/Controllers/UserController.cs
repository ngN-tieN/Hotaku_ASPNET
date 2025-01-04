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
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userService.GetAllUsers();
                return Ok(new { code = 1, data = users });
            }
            catch (Exception ex)
            {
                return NotFound(new { code = 0, message = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await userService.GetUserById(userId);
                return Ok(new { code = 1, data = user });
            }
            catch (Exception ex)
            {
                return NotFound(new { code = 0, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(User user)
        {
            try
            {
                var message = await userService.DeleteUser(user);
                return Ok(new { code = 1, message });
            }
            catch (Exception ex)
            {
                return NotFound(new { code = 0, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                user.CreatedAt = DateTime.Now;
                var newUser = await userService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, new { code = 1, data = newUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 0, message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                var updatedUser = await userService.UpdateUser(user);
                return Ok(new {code = 1, data = updatedUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 0, message = ex.Message });
            }
        }
    }
}