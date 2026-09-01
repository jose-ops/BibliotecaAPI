using Domain.Models;
using Microsoft.AspNetCore.Http;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<Livro>> ListarTodos();

        Task<Livro?> BuscarPorId(int id);

        Task AdicionarNovo(Livro livro);

        Task Atualizar(Livro livro);

        Task Remover(int id);


        Task<string?> UploadImagemLivroAsync(int livroId, IFormFile file);
    }
}
