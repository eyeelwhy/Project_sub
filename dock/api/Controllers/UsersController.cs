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
        
        [HttpPost("auth")]
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

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegDTO userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.Login) || 
                    string.IsNullOrEmpty(userDto.Password) || 
                    string.IsNullOrEmpty(userDto.Name))
                {
                    return BadRequest("Логин, пароль и имя являются обязательными полями");
                }
                else if (userDto.Password.Length < 8 || userDto.Login.Length < 8){
                   return BadRequest("Длина полей Пароль или Логин меньше 8");
                }
                bool userExists = Program.context.Users.Any(u => u.Login == userDto.Login);
                if (userExists)
                {
                    return Conflict("Пользователь с таким логином уже существует");
                }
                var newUser = new User
                {
                    Login = userDto.Login,
                    Password = userDto.Password,
                    Name = userDto.Name,
                    Email = userDto.Email,
                    PhoneNumber = userDto.PhoneNumber,
                    IdRole = 2 
                };
                Program.context.Users.Add(newUser);
                Program.context.SaveChanges();
                return Ok(new 
                {
                    newUser.IdUser,
                    newUser.Login,
                    newUser.Name,
                    newUser.Email,
                    newUser.PhoneNumber,
                    newUser.IdRole
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                return StatusCode(500, "Произошла ошибка при регистрации пользователя");
            }
        }
    }
}