using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Usuarios
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataModificacao { get; set; }
    }
}
