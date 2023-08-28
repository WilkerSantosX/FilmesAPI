using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Data
{
    public class FilmeContext : DbContext
    {
        public FilmeContext(DbContextOptions<FilmeContext> options) 
            : base(options)
        {
                
        }

        public DbSet<Filme> filmes { get; set; }
    }
}
