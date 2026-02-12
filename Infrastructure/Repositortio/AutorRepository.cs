using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositortio
{
    public class AutorRepository : IAutorRepository
    {
        private readonly BibliotecaDbContext _context;

        public AutorRepository(BibliotecaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> ListarTodos()
        {
            return await _context.Autor
                .Include(a => a.Livros)
                .ToListAsync();
        }

        public async Task Adicionar(Autor autor)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(autor.Nome))
                throw new Exception("Nome do autor é obrigatório");

            if (string.IsNullOrWhiteSpace(autor.Nacionalidade))
                throw new Exception("Nacionalidade é obrigatória");

            await _context.Autor.AddAsync(autor);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Autor autor)
        {
            var autorExistente = await _context.Autor
            .FirstOrDefaultAsync();

            if(autorExistente == null)
                throw new Exception($"Autor com ID {autor.Id} não encontrado");

            // Atualiza apenas os dados do autor (não mexe nos livros aqui)
            autorExistente.Nome = autor.Nome;
            autorExistente.Nacionalidade = autor.Nacionalidade;

            _context.Autor.Update(autorExistente);
            await _context.SaveChangesAsync();
        }

        public async Task<Autor> BuscarPorId(int id)
        {
            var autor = await _context.Autor
            .Include(a => a.Livros) // IMPORTANTE: traz os livros do autor
            .FirstOrDefaultAsync(a => a.Id == id);

            if (autor == null)
                throw new Exception($"Autor com ID {id} não encontrado");

            return autor;
        }

       

        public async Task Remover(int id)
        {
            var autor = await _context.Autor
            .Include(a => a.Livros) // Importante verificar se tem livros
            .FirstOrDefaultAsync(a => a.Id == id);

            if (autor == null)
                throw new Exception($"Autor com ID {id} não encontrado");

            // OPÇÃO 1: Impedir deletar se tiver livros
            if (autor.Livros != null && autor.Livros.Any())
                throw new Exception($"Não é possível deletar. O autor possui {autor.Livros.Count} livro(s) cadastrado(s)");

            // OPÇÃO 2: Deletar em cascata (configurar no DbContext)
            // Os livros serão deletados automaticamente

            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();
        }
    }
}
