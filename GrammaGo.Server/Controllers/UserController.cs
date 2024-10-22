using Microsoft.AspNetCore.Mvc;
using GrammaGo.Server.Data;
using GrammaGo.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace GrammaGo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GrammaGoContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(GrammaGoContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }



        [HttpPut("update-personal-data/{id}")]
        public async Task<IActionResult> UpdatePersonalData(long id, [FromBody] PersonalDataUpdateRequest request)
        {
            if (request == null) 
            {
                return BadRequest("Datos de actualización no válidos.");
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

           
            usuario.Direccion = request.Direccion;
            usuario.Provincia = request.Provincia;
            usuario.CodigoPostal = request.CodigoPostal;

            
            await _context.SaveChangesAsync();
            return NoContent(); 
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico) || string.IsNullOrWhiteSpace(usuario.ContrasenaHash))
            {
                _logger.LogWarning("Faltan campos obligatorios en el registro.");
                return BadRequest(new { Error = "Correo electrónico y contraseña son requeridos." });
            }

            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);
            if (usuarioExistente != null)
            {
                _logger.LogWarning("Intento de registro con un correo ya existente.");
                return Conflict(new { Error = "Ya existe un usuario con este correo electrónico." });
            }

            try
            {
                usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.ContrasenaHash);
                usuario.FechaCreacion = DateTime.Now;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado exitosamente: " + usuario.CorreoElectronico);

                usuario.ContrasenaHash = null;
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el usuario.");
                return StatusCode(500, new { Error = "Error interno del servidor" });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico);

            if (usuario == null)
            {
                Console.WriteLine("Usuario no encontrado.");
                return Unauthorized("Usuario no encontrado.");
            }

            
            if (usuario.ContrasenaHash == null)
            {
                Console.WriteLine("La contraseña está vacía.");
                return Unauthorized("La contraseña está vacía.");
            }

            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.ContrasenaHash);
            if (!contrasenaValida)
            {
                Console.WriteLine("Contraseña incorrecta.");
                return Unauthorized("Contraseña incorrecta.");
            }

            
            Console.WriteLine("Inicio de sesión exitoso para el usuario: " + usuario.CorreoElectronico);
            return Ok(new { userId = usuario.Id, mensaje = "Inicio de sesión exitoso" }); 
        }

    }

    public class UsuarioLoginRequest
    {
        public required string CorreoElectronico { get; set; }
        public required string Contrasena { get; set; }
    }
}
