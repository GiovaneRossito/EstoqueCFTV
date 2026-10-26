using Microsoft.EntityFrameworkCore;
using EstoqueCFTV.Models; // Importa a pasta onde estão suas classes

namespace EstoqueCFTV.Data
{
    // A nossa classe herda de DbContext, que já vem com os poderes do Entity Framework
    public class AppDbContext : DbContext
    {
        // Esse construtor recebe as configurações (como qual banco usar) lá do Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Os DbSets são as representações das tabelas que serão criadas no SQL Server
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<MovimentacaoEstoque> Movimentacoes { get; set; }
    }
}