using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
           _config = config;   
        }

        //[HttpGet("Login")]

        //public IActionResult Login(string email, string password)
        //{
        //    var user = _userRepository.GetUser(email, password);
        //    if(user == null) { return BadRequest("No Such User"); }
        //    return Ok(user);
        //}

        [HttpPost("[action]")]
        public IActionResult Login(string Email,string Password)
        {
            var currentUser = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (currentUser == null)
            {
                return NotFound();
            }



            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            //if(securityKey.KeySize<256)
            //{
            //    securityKey= new SymmetricSecurityKey(new byte[256/8]);
            //}

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var   claims = new[]
            {
            new Claim(ClaimTypes.Email, currentUser.Email),
            new Claim("UserId",currentUser.Id.ToString()),
            new Claim(ClaimTypes.Role, currentUser.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }




        [HttpGet]
        public ActionResult <IEnumerable<UserDTO>> GetAllUsers()
        {
            var users =  _userRepository.GetAllUsers();
            var UserDTOs = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
                // IsAdmin is not included here
            });
            return Ok(UserDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var UserDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
                // IsAdmin is not included here
            };
            return Ok(UserDTO);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> AddUser(UserDTO UserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Name = UserDTO.Name,
                Email = UserDTO.Email,
                Password = UserDTO.Password,
                ConfirmPassword = UserDTO.ConfirmPassword
                
                // IsAdmin is not included here
            };

            await _userRepository.AddUserAsync(user);

            // Set the UserDTO.Id with the newly created user's Id
            UserDTO.Id = user.Id;

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, UserDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO UserDTO)
        {
            if (id != UserDTO.Id)
            {
                return BadRequest();
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Name = UserDTO.Name;
            user.Email = UserDTO.Email;

            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(id);

            return NoContent();
        }
    }
} 