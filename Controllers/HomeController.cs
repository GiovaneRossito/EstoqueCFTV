using System.Diagnostics;
using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ProdutoService _produtoService;
    private readonly OrdemDeServicoService _osService;
    private readonly MovimentacaoEstoqueService _movimentacaoService;
    private readonly ClienteService _clienteService;

    public HomeController(
        ProdutoService produtoService,
        OrdemDeServicoService osService,
        MovimentacaoEstoqueService movimentacaoService,
        ClienteService clienteService)
    {
        _produtoService = produtoService;
        _osService = osService;
        _movimentacaoService = movimentacaoService;
        _clienteService = clienteService;
    }

    public IActionResult Index()
    {
        var produtos = _produtoService.ListarTodos();
        var ordens = _osService.ListarTodos();
        var movimentacoes = _movimentacaoService.ListarTodos();
        var clientes = _clienteService.ListarTodos();

        var produtosEstoqueBaixo = produtos
            .Where(p => p.QuantidadeEstoque <= p.QuantidadeMinima)
            .OrderBy(p => p.QuantidadeEstoque)
            .ToList();

        var ordensAbertas = ordens
            .Where(os => os.Status != null &&
                (os.Status.Equals("Aberta", StringComparison.OrdinalIgnoreCase) ||
                 os.Status.Equals("Em andamento", StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(os => os.DataAbertura)
            .ToList();

        ViewBag.TotalProdutos = produtos.Count;
        ViewBag.TotalClientes = clientes.Count;
        ViewBag.QtdEstoqueBaixo = produtosEstoqueBaixo.Count;
        ViewBag.QtdOrdensAbertas = ordensAbertas.Count;

        ViewBag.ProdutosCriticos = produtosEstoqueBaixo.Take(5).ToList();
        ViewBag.UltimasMovimentacoes = movimentacoes
            .OrderByDescending(m => m.DataMovimentacao)
            .Take(5)
            .ToList();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}