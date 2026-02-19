using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Descricao
    {
        public int Id { get; set; }
        public string? Editora { get; set; }
        public string? Idioma { get; set; }
        public int NumeroPaginas { get; set; }
        public int AnoPublicacao { get; set; }

        public int LivroId { get; set; }
        public Livro? Livro { get; set; }
    }
}
