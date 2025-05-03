using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using User.Data;
using User.Models;
using UserModel = User.Models.User;
using User.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(UserModel user)
        {
            // Criptografando a senha antes de salvar no banco
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);

            // Adicionando o novo usuário ao banco
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Retornando o usuário criado
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User/login
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var isPasswordValid = PasswordHelper.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                return Unauthorized("JWT Secret Key is not configured.");
            }

            var token = JwtHelper.GenerateToken(user, secretKey);

            return Ok(new { token });
        }
        /* **/
        public class LoginRequest
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
            {
                return Unauthorized("User not identified.");
            }
    
            var user = await _context.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { user.Name, user.Email });
        }

        // POST: api/User/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Name == request.Name);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Gerar o token de recuperação (supondo que o método 'GeneratePasswordResetToken' existe)
            var token = PasswordResetToken.GeneratePasswordResetToken(user);
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(0.2);

            await _context.SaveChangesAsync();

            // Retornar o token gerado no Swagger
            return Ok(new { Token = token });    

            /* TODO: Aqui deve ser inserido uma forma de envio de e-mail com o token para inserir na tela de recuperacao**/    
        }
        /* **/
        public class ForgotPasswordRequest
        {
            public required string Email { get; set; }
            public required string Name { get; set; }  
        }

        // POST: api/User/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.PasswordResetTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Expired token or invalid.");
            }

            // Atualizando a senha
            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);

            // Limpar o token após a alteração
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;

            await _context.SaveChangesAsync();

            return Ok("Password changed successfully.");
        }
        /* **/
        public class ResetPasswordRequest
        {
            public required string Token { get; set; }
            public required string NewPassword { get; set; }
        }
    }
}
