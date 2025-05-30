using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbDojankwonContext>(optionts =>
    optionts.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));

// Configurar CORS para permitir cualquier origen, header y método
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddScoped<DBUsuario>();
builder.Services.AddScoped<ServiceUsuario>();
builder.Services.AddScoped<DBArticulo>();
builder.Services.AddScoped<ServiceArticulo>();
builder.Services.AddScoped<DBPrestamo>();
builder.Services.AddScoped<ServicePrestamo>();
builder.Services.AddScoped<DBRango>();
builder.Services.AddScoped<ServiceRango>();
builder.Services.AddScoped<DBGrupo>();
builder.Services.AddScoped<DBEstudiante>();
builder.Services.AddScoped<ServiceEstudiante>();
builder.Services.AddScoped<DBPago>();
builder.Services.AddScoped<ServicePago>();
builder.Services.AddScoped<DBExamen>();
builder.Services.AddScoped<ServiceExamen>();


QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

