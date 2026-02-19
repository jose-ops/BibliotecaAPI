using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDescricaoRepository
    {
        Task<IEnumerable<Descricao>> ListarTodos();

        Task<Descricao> BuscarPorId(int id);

        Task AdicionarNovo(Descricao descricao);

        Task Atualizar(Descricao descricao);

        Task Remover(int id);
    }
}
