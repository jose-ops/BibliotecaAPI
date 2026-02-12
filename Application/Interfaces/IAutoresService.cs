using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAutoresService
    {
        Task<IEnumerable<Autor>> ListarTodos();
        Task<Autor> BuscarPorId(int id);
        Task AdicionarNovo(Autor autor);
        Task Atualizar(Autor autor);
        Task Remover(int id);
    }
}
