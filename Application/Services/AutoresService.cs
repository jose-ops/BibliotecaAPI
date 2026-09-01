using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AutoresService : IAutoresService
    {
        private readonly IAutorRepository _repo;

        public AutoresService(IAutorRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Autor>> ListarTodos() => _repo.ListarTodos();

        public Task<Autor?> BuscarPorId(int id) => _repo.BuscarPorId(id);

        public Task AdicionarNovo(Autor autor ) => _repo.Adicionar(autor);
        public Task Atualizar(Autor autor) => _repo.Atualizar(autor);

        public Task Remover(int id) => _repo.Remover(id);
       
    }
}
