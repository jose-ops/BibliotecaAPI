using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAutorRepository
    {
        //get all
        Task<IEnumerable<Autor>> ListarTodos();

        //get id
        Task<Autor?> BuscarPorId(int id);

        //post
        Task Adicionar(Autor autor);

        //put
        Task Atualizar(Autor autor);

        //delete
        Task Remover(int id);
    }
}
