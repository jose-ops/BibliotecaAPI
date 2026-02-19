using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDescricaoService
    {
        Task<IEnumerable<Descricao>> ListarTodos();
        Task<Descricao> BuscarPorId(int id);
        Task AdicionarNovo(Descricao descricao);
        Task Atualizar(Descricao descricao);
        Task Remover(int id);
    }
}