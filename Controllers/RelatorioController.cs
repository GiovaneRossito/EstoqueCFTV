using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class RelatorioController : Controller
{
    private readonly AppDbContext _context;

    public RelatorioController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var resumoOS = _context.OrdemDeServicos
            .Include(os => os.Cliente)
            .Include(os => os.Usuario)
            .Include(os => os.ServicosRealizados)
            .Include(os => os.MateriaisUtilizados)
                .ThenInclude(m => m.Produto)
            .ToList()
            .Select(os =>
            {
                decimal totalServicos = os.ServicosRealizados.Sum(s => s.Quantidade * s.ValorCobrado);
                decimal totalMateriais = os.MateriaisUtilizados
                    .Where(m => m.Tipo == TipoMovimentacao.Saida && m.Produto != null)
                    .Sum(m => m.Quantidade * m.Produto!.PrecoUnitario);

                return new
                {
                    os.Id,
                    Cliente = os.Cliente?.Nome ?? "—",
                    Responsavel = os.Usuario?.Nome ?? "—",
                    os.Status,
                    TotalServicos = totalServicos,
                    TotalMateriais = totalMateriais,
                    TotalGeral = totalServicos + totalMateriais
                };
            })
            .ToList();
        
        var agrupamentoCategorias = _context.Produtos
            .Include(p => p.Categoria)
            .ToList()
            .GroupBy(p => p.Categoria?.Nome ?? "Sem Categoria")
            .Select(g => new
            {
                Categoria = g.Key,
                QtdProdutos = g.Count(),
                TotalItensEstoque = g.Sum(p => p.QuantidadeEstoque),
                ValorTotalEstoque = g.Sum(p => p.QuantidadeEstoque * p.PrecoUnitario),
                PrecoMedio = g.Average(p => p.PrecoUnitario)
            })
            .OrderByDescending(x => x.ValorTotalEstoque)
            .ToList();
        
        var produtos = _context.Produtos.OrderBy(p => p.Nome).ToList();
        var movimentacoes = _context.Movimentacoes.ToList();

        var pivotProdutos = produtos.Select(p =>
        {
            int entradas = movimentacoes
                .Where(m => m.ProdutoId == p.Id && m.Tipo == TipoMovimentacao.Entrada)
                .Sum(m => m.Quantidade);

            int saidas = movimentacoes
                .Where(m => m.ProdutoId == p.Id && m.Tipo == TipoMovimentacao.Saida)
                .Sum(m => m.Quantidade);

            return new
            {
                p.SKU,
                p.Nome,
                Entradas = entradas,
                Saidas = saidas,
                SaldoAtual = p.QuantidadeEstoque
            };
        }).ToList();

        ViewBag.ResumoOS = resumoOS;
        ViewBag.FaturamentoGeralOS = resumoOS.Sum(x => x.TotalGeral);
        ViewBag.AgrupamentoCategorias = agrupamentoCategorias;
        ViewBag.PivotProdutos = pivotProdutos;

        return View();
    }
}