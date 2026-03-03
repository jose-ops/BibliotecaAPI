 using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Infrastructure.Data
{
    public class BibliotecaDbContext : DbContext
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Descricao> Descricao { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(l => l.AutorId);

            modelBuilder.Entity<Descricao>()
                .HasOne(l => l.Livro)
                .WithOne(d => d.Descricao)
                .HasForeignKey<Descricao>(l => l.LivroId);

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Autor>().HasData(
                   new Autor { Id = 1, Nome = "Machado de Assis", Nacionalidade = "Brasileiro" },
                   new Autor { Id = 2, Nome = "Clarice Lispector", Nacionalidade = "Brasileira" },
                   new Autor { Id = 3, Nome = "J.K. Rowling", Nacionalidade = "Britânica" }
);

            modelBuilder.Entity<Livro>().HasData(
                new Livro
                {
                    Id = 1,
                    Titulo = "Dom Casmurro",
                    Disponivel = true,

                    AutorId = 1
                },
                new Livro
                {
                    Id = 2,
                    Titulo = "A Hora da Estrela",
                    Disponivel = true,

                    AutorId = 2
                },
                new Livro
                {
                    Id = 3,
                    Titulo = "Harry Potter e a Pedra Filosofal",
                    Disponivel = false,

                    AutorId = 3
                }
            );

            modelBuilder.Entity<Usuarios>().HasData(
                new Usuarios
                {
                    Id = 1,
                    Email = "admin@biblioteca.com",
                    Nome = "Administrador",
                    PasswordHash = "$2a$11$hyDuva19sclrEh1ERvI1VuyOktQEwm6kLIm0kcYjkZQwxwyxQq4Ru",
                    Role = "Admin",
                    DataCriacao = new DateTime(2026, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                }
            );


            base.OnModelCreating(modelBuilder);

        }
    }
}