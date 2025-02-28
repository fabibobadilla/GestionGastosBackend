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
            // Validación de usuario
            var user = _context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == loginModel.Email.ToLower());
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
            {
                return Unauthorized("Credenciales inválidas");
            }
            // Generar el token JWT
            var token = GenerateJwtToken(user);

            return Ok(new { message = "Inicio de sesión correcto", token });
            //return Ok(token);
        }

        // POST: api/Auth/Register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            // Validación si el usuario ya existe

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            // Generar token JWT
            var token = GenerateJwtToken(usuario);
            return Ok(new { message = "Usuario registrado exitosamente", token });
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
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            return Ok("Token válido");
        }


    }
}

