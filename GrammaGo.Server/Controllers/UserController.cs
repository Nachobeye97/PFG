using Microsoft.AspNetCore.Mvc;
using GrammaGo.Server.Data;
using GrammaGo.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;  // Usamos BCrypt para hashear y verificar la contraseña

namespace GrammaGo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GrammaGoContext _context;

        public UserController(GrammaGoContext context)
        {
            _context = context;
        }

        // Endpoint para registrar un nuevo usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico) || string.IsNullOrWhiteSpace(usuario.ContrasenaHash))
            {
                return BadRequest("Correo electrónico y contraseña son requeridos.");
            }

            // Verificar si ya existe
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);
            if (usuarioExistente != null)
            {
                return Conflict("Ya existe un usuario con este correo electrónico.");
            }

            try
            {
                // Hashear la contraseña
                usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.ContrasenaHash);

                usuario.FechaCreacion = DateTime.Now;  // Aquí asignas la fecha actual

                // Guardar en la base de datos
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                Console.WriteLine("Usuario guardado exitosamente.");

                // Devolver el usuario registrado sin la contraseña hasheada
                usuario.ContrasenaHash = null;
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Log completo del error para mayor claridad
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                Console.WriteLine(ex.StackTrace); // Esto te dará más detalles de la traza del error
                return StatusCode(500, "Error interno del servidor");
            }
        }


        // Nuevo endpoint para el inicio de sesión
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            // Verificar si el usuario existe en la base de datos
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico);

            if (usuario == null)
            {
                Console.WriteLine("Usuario no encontrado.");
                return Unauthorized("Usuario no encontrado.");
            }

            // Verificar si la contraseña es correcta
            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.ContrasenaHash);
            if (!contrasenaValida)
            {
                Console.WriteLine("Contraseña incorrecta.");
                return Unauthorized("Contraseña incorrecta.");
            }

            // Autenticación exitosa
            Console.WriteLine("Inicio de sesión exitoso para el usuario: " + usuario.CorreoElectronico);
            return Ok("Inicio de sesión exitoso");
        }
    }

    // Clase auxiliar para las credenciales de inicio de sesión
    public class UsuarioLoginRequest
    {
        public required string CorreoElectronico { get; set; }
        public required string Contrasena { get; set; }
    }
}
