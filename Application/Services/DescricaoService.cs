using Application.Interfaces;
using Biblioteca.Domain.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DescricaoService(IDescricaoRepository repo) : IDescricaoService
    {
        private readonly IDescricaoRepository _repo = repo;


        public Task<IEnumerable<Descricao>> ListarTodos() => _repo.ListarTodos();

        public Task<Descricao> BuscarPorId(int id) => _repo.BuscarPorId(id);

        public Task AdicionarNovo(Descricao descricao) => _repo.AdicionarNovo(descricao);

        public  Task Atualizar(Descricao descricao) => _repo.Atualizar(descricao);

        public Task Remover(int id) => _repo.Remover(id);

    }
}
