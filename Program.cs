using GrammaGo.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Configurar la base de datos
builder.Services.AddDbContext<GrammaGoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();

// Configuración de Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar autenticación JWT con JwtBearerDefaults
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "GrammaGo", // Emisor
        ValidAudience = "GrammaGoFrontend", // Audiencia
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estaEsUnaClaveSecretaMuyLargaParaElTokenDeJWT123456")) // Llave secreta
    };

    // Opción para asegurar que el token esté presente en las solicitudes
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
                {
                    context.Token = token.Substring("Bearer ".Length).Trim();
                }
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Middleware de autenticación y autorización
app.UseAuthentication();  // Habilitar la autenticación
app.UseAuthorization();   // Habilitar la autorización

// Configuración del pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // Redirigir a HTTPS si no se está utilizando

app.UseStaticFiles();  // Servir archivos estáticos
app.UseCors("AllowAll"); // Usar la política CORS que permite todas las solicitudes

app.MapControllers();  // Mapear las rutas a los controladores
app.MapFallbackToFile("/index.html");  // Fallback para el frontend de una SPA

app.Run();  // Ejecutar la aplicación
