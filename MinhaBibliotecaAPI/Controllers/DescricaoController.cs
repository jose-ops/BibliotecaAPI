using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinhaBibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescricaoController : ControllerBase
    {
        private readonly IDescricaoService _service;

        public DescricaoController(IDescricaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ListarTodos());


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var descricao = await _service.BuscarPorId(id);
            return descricao == null ? NotFound() : Ok(descricao);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(Descricao descricao)
        {
            await _service.AdicionarNovo(descricao);
            return CreatedAtAction(nameof(Get), new { id = descricao.Id }, descricao);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Descricao descricao)
        {
            if (descricao == null) return BadRequest();
            await _service.Atualizar(descricao);
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
