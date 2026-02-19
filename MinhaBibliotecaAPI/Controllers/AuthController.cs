using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MinhaBibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BibliotecaDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(
            BibliotecaDbContext context,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Verifica se o email já está cadastrado
                if (await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return BadRequest(new { message = "Email já cadastrado no sistema" });
                }

                // Cria o novo usuário
                var user = new Usuarios
                {
                    Email = registerDto.Email.ToLower().Trim(),
                    Nome = registerDto.Nome.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Role = "User", // Usuário comum por padrão
                    DataCriacao = DateTime.UtcNow
                };

                // Adiciona ao banco
                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                // Gera o token JWT
                var token = _tokenService.GenerateToken(user);
                var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"]);

                // Retorna os dados do usuário + token
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    Nome = user.Nome,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao registrar usuário", error = ex.Message });
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email.ToLower().Trim());

                
                if (user == null)
                {
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                // Verifica se a senha está correta
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                // Gera o token JWT
                var token = _tokenService.GenerateToken(user);
                var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"]);

                // Retorna os dados do usuário + token
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    Nome = user.Nome,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao fazer login", error = ex.Message });
            }
        }
    }
}
