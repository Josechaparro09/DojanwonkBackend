using BLL;
using DAL.Modelos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbDojankwonContext>(optionts=>
optionts.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));
// Add services to the container.

builder.Services.AddScoped<DBUsuario>();
builder.Services.AddScoped<ServiceUsuario>();
builder.Services.AddScoped<DBArticulo>();
builder.Services.AddScoped<ServiceArticulo>();
builder.Services.AddScoped<DBPrestamo>();
builder.Services.AddScoped<ServicePrestamo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
