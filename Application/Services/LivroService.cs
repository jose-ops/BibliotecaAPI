using Application.Interfaces;
using Biblioteca.Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LivroService(ILivroRepository repo, IS3Service s3Service) : ILivroService
    {
        private readonly ILivroRepository _repo = repo;
        private readonly IS3Service _s3Service = s3Service;

        //get all
        public Task<IEnumerable<Livro>> ListarTodos() => _repo.ListarTodos();

        //get id
        public Task<Livro?> BuscarPorId(int id) => _repo.BuscarPorId(id);

        //post
        public Task AdicionarNovo(Livro livro) => _repo.Adicionar(livro);

        public async Task<string?> UploadImagemLivroAsync(int id, IFormFile file)
        {
            // Busca o livro
            var livro = await _repo.GetByIdAsync(id);
            if (livro == null)
                return null;

            if (file.Length > 5 * 1024 * 1024) // 5MB
                throw new ArgumentException("Arquivo muito grande. Máximo 5MB");

            // Faz upload pro S3
            var url = await _s3Service.UploadImagemLivroAsync(id, file);

            // Atualiza a URL no banco
            livro.ImageUrl = url;
            await _repo.SaveChangesAsync();

            return url;
        }

        //put
        public Task Atualizar(Livro livro) => _repo.Atualizar(livro);

        //delete
        public Task Remover(int id) => _repo.Remover(id);

       
    }
}
