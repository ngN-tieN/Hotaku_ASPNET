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
        public List<User> GetAllUsers()
        {
            return userService.GetAllUsers();
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(string userId)
        {
            try
            {
                var user = userService.GetUserById(userId);

                return Ok(user);
            }
            catch (Exception ex) {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(User user)
        {
            try
            {
                var message = userService.DeleteUser(user);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                user.CreatedAt = DateTime.Now;
                var newUser = userService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            try
            {
                var updatedUser = userService.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}