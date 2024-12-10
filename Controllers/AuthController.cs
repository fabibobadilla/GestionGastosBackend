using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionGastos.Models;
using Microsoft.AspNetCore.Authorization;
using GestionGastos.DataContext;
using Microsoft.CodeAnalysis.Scripting;

namespace GestionGastos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GestionGastosContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(GestionGastosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Validación de usuario (simulación)
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == loginModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
            {
                return Unauthorized("Credenciales inválidas");
            }

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // POST: api/Auth/Register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            // Validación si el usuario ya existe
            if (_context.Usuarios.Any(u => u.Email == registerModel.Email))
            {
                return BadRequest("El usuario ya existe.");
            }

            // Crear el nuevo usuario
            var usuario = new Usuario
            {
                Nombre = registerModel.Nombre,
                Email = registerModel.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerModel.Password), // Hash de la contraseña
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuario registrado exitosamente.");
        }

        // Método privado para generar el JWT token
        private string GenerateJwtToken(Usuario user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

