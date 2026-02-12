using Biblioteca.Domain.Interfaces;
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
    public class LivroRepository : ILivroRepository
    {
        private readonly BibliotecaDbContext _context;

        public LivroRepository(BibliotecaDbContext context)
        {
            _context = context;
        }
        //get all
        public async Task<IEnumerable<Livro>> ListarTodos()
        {
            return await _context.Livros
                     .Include(l => l.Autor)
                     .Include(d => d.Descricao)
                     .ToListAsync();
        }

        //get id
        public async Task<Livro> BuscarPorId(int id) =>
            await _context.Livros
            .Include(l => l.Autor)
            .Include(d => d.Descricao)
            .FirstOrDefaultAsync(l => l.Id == id)

            ?? throw new InvalidOperationException($"Livro com Id {id} não encontrado!");

        //post
        public async Task Adicionar(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .Include(d => d.Descricao)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //put
        public async Task Atualizar(Livro livro)
        {
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();
        }

        //delete
        public async Task Remover(int id)
        {
            var livro = await BuscarPorId(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
            }
        }

    }
}
