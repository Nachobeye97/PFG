using Microsoft.AspNetCore.Mvc;
using GrammaGo.Server.Data;
using GrammaGo.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System;

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

        // Obtener usuario por ID
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

        // Actualizar datos personales del usuario
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

        // Registrar un nuevo usuario
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
                usuario.Role = "cliente"; // Por defecto, el rol es "cliente"

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

        // Iniciar sesión del usuario y generar JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.ContrasenaHash))
            {
                return Unauthorized("Usuario no encontrado o contraseña incorrecta.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Role))
            {
                return Unauthorized("El rol del usuario no está definido.");
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estaEsUnaClaveSecretaMuyLargaParaElTokenDeJWT123456"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "GrammaGo",
                audience: "GrammaGoFrontend",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Console.WriteLine("Inicio de sesión exitoso para el usuario: " + usuario.CorreoElectronico);
            Console.WriteLine($"User Role: {usuario.Role}");
            Console.WriteLine($"User ID: {usuario.Id}");
            Console.WriteLine($"Token generado: {tokenString}");

            return Ok(new { userId = usuario.Id, token = tokenString, role = usuario.Role });
        }

      [HttpGet("exercises")]
public async Task<IActionResult> GetExercises([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string level = null)
{
    try
    {
        // Obtener los ejercicios y filtrarlos por nivel si se pasa un nivel
        var exercisesQuery = _context.Exercises.AsQueryable();

        // Filtrar por nivel si se pasa un nivel
        if (!string.IsNullOrEmpty(level))
        {
            if (int.TryParse(level, out int parsedLevel))  // Convertir el nivel de string a int
            {
                exercisesQuery = exercisesQuery.Where(e => e.Level == parsedLevel);
            }
            else
            {
                return BadRequest("El nivel debe ser un número entero.");
            }
        }

        // Realizar la paginación y mezclar aleatoriamente
        var exercises = await exercisesQuery
            .OrderBy(r => Guid.NewGuid()) // Aleatorio
            .Skip((page - 1) * pageSize) // Salta los primeros (page - 1) * pageSize
            .Take(pageSize) // Toma solo los ejercicios necesarios
            .ToListAsync();

        return Ok(exercises);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error al cargar los ejercicios: {ex.Message}");
        return StatusCode(500, "Error interno en el servidor.");
    }
}





        // Crear un nuevo ejercicio
        [HttpPost("exercises/add")]
        public async Task<IActionResult> AddExercise([FromBody] Exercise exercise)
        {
            if (exercise == null)
            {
                return BadRequest("Ejercicio no válido.");
            }

            // Verificar que el Level sea 1 o 2
            if (exercise.Level != 1 && exercise.Level != 2)
            {
                return BadRequest("Nivel no válido. Debe ser 1 o 2.");
            }

            // Verificar si ya existe un ejercicio con la misma pregunta
            var existingExercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Question.ToLower().Trim() == exercise.Question.ToLower().Trim());

            if (existingExercise != null)
            {
                return Conflict(new { message = "El ejercicio ya existe." });
            }

            // Obtener el ID del usuario a partir de los claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
            {
                Console.WriteLine("Access Denied: User ID or role claim is missing.");
                return Forbid();
            }

            // Verificar si el rol del usuario es "admin"
            if (userRoleClaim.Value != "admin")
            {
                return new ObjectResult(new { message = "Access Denied: Only admin users can create exercises." })
                {
                    StatusCode = 403
                };
            }

            if (!long.TryParse(userIdClaim.Value, out long userId))
            {
                Console.WriteLine("Invalid User ID.");
                return BadRequest(new { message = "Invalid User ID." });
            }

            exercise.UserId = userId;

            // Guardar el ejercicio en la base de datos
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return Ok(exercise);
        }

        // Guardar los resultados del ejercicio
        [HttpPost("results")]
        public async Task<IActionResult> SaveResults([FromBody] ExerciseResult result)
        {
            if (result == null)
            {
                return BadRequest("Los datos del resultado son inválidos.");
            }

            // Validar los campos
            if (result.AttemptNumber <= 0)
            {
                return BadRequest("El número de intento debe ser mayor que cero.");
            }

            var userId = GetUserIdFromClaims(); // Obtener el ID del usuario desde los claims
            result.UserId = userId; // Asignar el ID del usuario al resultado
            result.Date = DateTime.UtcNow; // Asignar la fecha de finalización

            // Obtener el número de intentos del usuario actual
            var attemptsCount = await _context.ExerciseResults.CountAsync(r => r.UserId == userId);

            // Asignar el número de intento
            result.AttemptNumber = attemptsCount > 0 ? attemptsCount + 1 : 1; // Reiniciar a 1 si no hay intentos previos

            // Guardar el resultado
            try
            {
                _context.ExerciseResults.Add(result);
                await _context.SaveChangesAsync();
                return Ok(result); // Retornar el resultado guardado
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al guardar el resultado: " + ex.Message);
            }
        }

      [HttpGet("results")]
public async Task<IActionResult> GetUserResults()
{
    try
    {
        var userId = GetUserIdFromClaims();  // Asegúrate de que este método esté funcionando correctamente
        var results = await _context.ExerciseResults
            .Where(r => r.UserId == userId)
            .Select(r => new
            {
                r.Id,
                r.AttemptNumber,
                r.CorrectCount,
                r.IncorrectCount,
                r.Level,
                Date = r.Date
            })
            .ToListAsync();

        return Ok(results);  // Retornar los resultados de manera correcta
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error al obtener los resultados de ejercicios: {ex.Message}");
        return StatusCode(500, "Error interno del servidor");
    }
}


        private long GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("User ID not found in claims");
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return long.Parse(userIdClaim.Value);
        }
    }

    public class UsuarioLoginRequest
    {
        public required string CorreoElectronico { get; set; }
        public required string Contrasena { get; set; }
    }
}
