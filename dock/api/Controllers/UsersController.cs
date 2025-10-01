using api_docker.ModelDb;
using dockers_api.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_docker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = Program.context.Users.ToList();
            return Ok(users);
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserAuth user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password)) 
            { 
                return NotFound();
            }
            User? userAuth = Program.context.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            if (userAuth == null)
            {
                return NotFound();
            }
            return Ok(userAuth);
        }

    }
}
