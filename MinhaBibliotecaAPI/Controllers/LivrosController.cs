using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MinhaBibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _service;

        public LivrosController(ILivroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ListarTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var livro = await _service.BuscarPorId(id);
            return livro == null ? NotFound() : Ok(livro);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(Livro livro)
        {

            await _service.AdicionarNovo(livro);
            return CreatedAtAction(nameof(Get), new { id = livro.Id }, livro);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/upload")]
        public async Task<ActionResult> UploadImagem(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            try
            {
                var url = await _service.UploadImagemLivroAsync(id, file);

                if (url == null)
                    return NotFound("Livro não encontrado ou erro no upload");

                return Ok(new { ImageUrl = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Livro livro)
        {
            if (id != livro.Id) return BadRequest();
            await _service.Atualizar(livro);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }

    }
}
