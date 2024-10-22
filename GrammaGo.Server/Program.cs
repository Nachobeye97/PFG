using GrammaGo.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar base de datos
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

// Add services to the container.
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar autenticación (ejemplo con JWT)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://your-auth-domain";
        options.Audience = "your-api-audience";
    });

var app = builder.Build();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configuración del pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll"); // Usar la política CORS que permite todas las solicitudes

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
