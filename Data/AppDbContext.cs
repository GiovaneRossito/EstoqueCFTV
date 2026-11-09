﻿using System.Collections.Generic;
 using EstoqueCFTV.Models;
 using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<MovimentacaoEstoque> Movimentacoes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<OrdemDeServico> OrdemDeServicos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ItemServicoOS> ItemServicos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ItemServicoOS>().HasKey(iso => new { iso.OrdemDeServicoId, iso.ServicoId });

            modelBuilder.Entity<ItemServicoOS>()
                .HasOne(ios => ios.OrdemDeServico)
                .WithMany(os => os.ServicosRealizados)
                .HasForeignKey(ios => ios.OrdemDeServicoId);

            modelBuilder.Entity<ItemServicoOS>()
                .HasOne(ios => ios.Servico)
                .WithMany(s => s.Ordens)
                .HasForeignKey(ios => ios.ServicoId);
        }
    }
}