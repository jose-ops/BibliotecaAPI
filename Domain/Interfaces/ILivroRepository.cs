using Domain.Models;

namespace Biblioteca.Domain.Interfaces;

public interface ILivroRepository
{
    Task<IEnumerable<Livro>> ListarTodos();
    Task<Livro> BuscarPorId(int id);
    Task Adicionar(Livro livro);
    Task Atualizar(Livro livro);
    Task Remover(int id);

    Task<Livro?> GetByIdAsync(int id);
    Task<bool> SaveChangesAsync();
}
