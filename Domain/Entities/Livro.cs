using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public bool Disponivel { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

      
        public Descricao? Descricao { get; set; }

        public int AutorId { get; set; }

        public Autor? Autor { get; set; }
    }
}
