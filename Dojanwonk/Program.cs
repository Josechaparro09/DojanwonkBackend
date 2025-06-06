using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ SOLUCIÓN: Clave JWT de exactamente 32 bytes (256 bits) para HS256
var jwtKey = "mi-clave-super-secreta-de-32bytes"; // Exactamente 32 caracteres
var key = Encoding.UTF8.GetBytes(jwtKey);

// ✅ Configuración mejorada de JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Solo para desarrollo
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "DojankwonAPI", // ✅ Mismo issuer que TokenService
        ValidateAudience = true,
        ValidAudience = "DojankwonClient", // ✅ Mismo audience que TokenService
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Elimina tolerancia de tiempo
    };
});

// ✅ AUTORIZACIÓN POR ROLES - Sistema Dojang
builder.Services.AddAuthorization(options =>
{
    // ✅ Solo Administrador
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireRole("Administrador"));

    // ✅ Administrador o Instructor (para funciones pedagógicas)
    options.AddPolicy("AdminOInstructor", policy =>
        policy.RequireRole("Administrador", "Instructor"));

    // ✅ Administrador o Recepcionista (para funciones administrativas)
    options.AddPolicy("AdminORecepcion", policy =>
        policy.RequireRole("Administrador", "Recepcionista"));

    // ✅ Cualquier staff (todos los roles)
    options.AddPolicy("TodoStaff", policy =>
        policy.RequireRole("Administrador", "Instructor", "Recepcionista"));

    // ✅ Solo Instructor (para evaluaciones y enseñanza)
    options.AddPolicy("SoloInstructor", policy =>
        policy.RequireRole("Instructor"));

    // ✅ Solo Recepcionista (para tareas específicas de recepción)
    options.AddPolicy("SoloRecepcion", policy =>
        policy.RequireRole("Recepcionista"));
});

// ✅ DbContext
builder.Services.AddDbContext<DbDojankwonContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));

// ✅ CORS configurado (considera restringir en producción)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Servicios organizados por categorías
// Servicios de Usuario
builder.Services.AddScoped<DBUsuario>();
builder.Services.AddScoped<ServiceUsuario>();

// Servicios de Inventario
builder.Services.AddScoped<DBArticulo>();
builder.Services.AddScoped<ServiceArticulo>();
builder.Services.AddScoped<DBPrestamo>();
builder.Services.AddScoped<ServicePrestamo>();

// Servicios de Estudiantes
builder.Services.AddScoped<DBEstudiante>();
builder.Services.AddScoped<ServiceEstudiante>();
builder.Services.AddScoped<DBRango>();
builder.Services.AddScoped<ServiceRango>();
builder.Services.AddScoped<DBGrupo>();

// Servicios de Pagos y Exámenes
builder.Services.AddScoped<DBPago>();
builder.Services.AddScoped<ServicePago>();
builder.Services.AddScoped<DBExamen>();
builder.Services.AddScoped<ServiceExamen>();

// Servicios Auxiliares
builder.Services.AddScoped<NotificacionesCorreo>();
builder.Services.AddScoped<TokenService>();

// ✅ Configuración QuestPDF
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// ✅ Servicios de API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Middleware en orden correcto
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ✅ Debe ir antes de UseAuthorization
app.UseAuthorization();   // ✅ Debe ir después de UseAuthentication

app.MapControllers();

app.Run();