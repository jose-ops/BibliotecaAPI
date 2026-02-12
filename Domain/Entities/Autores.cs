using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Nacionalidade { get; set; }

        [JsonIgnore]
        public ICollection<Livro> Livros { get; set; } = [];

    }
}
