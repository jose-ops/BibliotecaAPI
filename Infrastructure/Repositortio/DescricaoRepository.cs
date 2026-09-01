using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositortio
{
    public class DescricaoRepository : IDescricaoRepository
    {
        private readonly BibliotecaDbContext _context;

        public DescricaoRepository(BibliotecaDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Descricao>> ListarTodos()
        {
            return await _context.Descricao.ToListAsync();
        }

        public async Task<Descricao?> BuscarPorId(int id)
        {
            return await _context.Descricao.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AdicionarNovo(Descricao descricao)
        {
            await _context.Descricao.AddAsync(descricao);
            await _context.SaveChangesAsync();
        }

        public Task Atualizar(Descricao descricao)
        {
            return Task.Run(() =>
            {
                _context.Descricao.Update(descricao);
                _context.SaveChanges();
            });
        }

        public Task Remover(int id)
        {
            return Task.Run(() =>
            {
                var descricao = _context.Descricao.FirstOrDefault(d => d.Id == id)
                    ?? throw new InvalidOperationException($"Descrição com Id {id} não encontrado!");
                _context.Descricao.Remove(descricao);
                _context.SaveChanges();
            });
        }
    }
}
