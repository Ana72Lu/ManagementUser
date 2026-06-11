using Microsoft.EntityFrameworkCore;
using ManagementUser.Data; // O namespace onde está seu AppDbContext
using ManagementUser.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do MySQL com Pomelo
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

// Injeção de Dependência dos Serviços
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PerfilService>();

builder.Services.AddEndpointsApiExplorer(); // Necessário para controllers mínimos
builder.Services.AddSwaggerGen(); // Configuração básica do Swagger

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
