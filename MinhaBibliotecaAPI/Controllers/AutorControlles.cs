using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MinhaBibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AutoresController : ControllerBase
    {
        private readonly IAutoresService _service;
        public AutoresController(IAutoresService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ListarTodos());


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var autor = await _service.BuscarPorId(id);
            return autor == null ? NotFound() : Ok(autor);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Autor autor)
        {
            await _service.AdicionarNovo(autor);
            return CreatedAtAction(nameof(Get), new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id) return BadRequest();
            await _service.Atualizar(autor);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }
    }

}
