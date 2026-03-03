using Application.Interfaces;
using Application.Services;
using Biblioteca.Domain.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositortio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .LogTo(Console.WriteLine, LogLevel.Information)//logs
    );

// Injeção de dependência
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<ILivroService, LivroService>();

builder.Services.AddScoped<IDescricaoRepository, DescricaoRepository>();
builder.Services.AddScoped<IDescricaoService, DescricaoService>();

builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<IAutoresService, AutoresService>();

builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthorization();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = jwtSettings["Issuer"],
            //ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero // Remove o delay padrão de 5 minutos
        };
    });

builder.Services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp =>
{
    var configuration = builder.Configuration;
    var hostName = configuration["RabbitMQ:HostName"] ?? throw new InvalidOperationException("RabbitMQ:HostName configuration is missing.");
    var userName = configuration["RabbitMQ:UserName"] ?? throw new InvalidOperationException("RabbitMQ:UserName configuration is missing.");
    var password = configuration["RabbitMQ:Password"] ?? throw new InvalidOperationException("RabbitMQ:Password configuration is missing.");

    return new RabbitMQ.Client.ConnectionFactory
    {
        HostName = hostName,
        UserName = userName,
        Password = password
    };
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        {
            policy.WithOrigins(
            "http://localhost:3000",      // Create React App
            "http://localhost:5173",      // Vite
            "http://localhost:5174"       // Vite alternativo
            )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseHttpsRedirection();


app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
