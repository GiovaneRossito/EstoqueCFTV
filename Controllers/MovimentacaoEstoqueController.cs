using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class MovimentacaoEstoqueController : Controller
{
    private readonly MovimentacaoEstoqueService _service;
    private readonly ProdutoService _produtoService;
    private readonly UsuarioService _usuarioService;
    private readonly ClienteService _clienteService;

    public MovimentacaoEstoqueController(
        MovimentacaoEstoqueService service,
        ProdutoService produtoService,
        UsuarioService usuarioService,
        ClienteService clienteService)
    {
        _service = service;
        _produtoService = produtoService;
        _usuarioService = usuarioService;
        _clienteService = clienteService;
    }

    public IActionResult Index(string? busca, int? produtoId, TipoMovimentacao? tipo, DateTime? dataInicial, DateTime? dataFinal)
    {
        var movimentacoes = _service.ListarTodos().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            string termo = busca.Trim();
            movimentacoes = movimentacoes.Where(m =>
                (m.Observacao != null && m.Observacao.Contains(termo, StringComparison.OrdinalIgnoreCase)) ||
                (m.Cliente != null && m.Cliente.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)) ||
                (m.Usuario != null && m.Usuario.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)));
        }

        if (produtoId.HasValue && produtoId.Value > 0)
            movimentacoes = movimentacoes.Where(m => m.ProdutoId == produtoId.Value);

        if (tipo.HasValue)
            movimentacoes = movimentacoes.Where(m => m.Tipo == tipo.Value);

        if (dataInicial.HasValue)
            movimentacoes = movimentacoes.Where(m => m.DataMovimentacao.Date >= dataInicial.Value.Date);

        if (dataFinal.HasValue)
            movimentacoes = movimentacoes.Where(m => m.DataMovimentacao.Date <= dataFinal.Value.Date);

        ViewBag.Produtos = new SelectList(_produtoService.ListarTodos(), "Id", "Nome");
        ViewBag.BuscaAtual = busca;
        ViewBag.ProdutoAtual = produtoId;
        ViewBag.TipoAtual = tipo;
        ViewBag.DataInicial = dataInicial?.ToString("yyyy-MM-dd");
        ViewBag.DataFinal = dataFinal?.ToString("yyyy-MM-dd");

        return View(movimentacoes.OrderByDescending(m => m.DataMovimentacao).ToList());
    }

    public IActionResult Create()
    {
        CarregarDadosAuxiliares();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MovimentacaoEstoque movimentacao)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(movimentacao);
        }

        try
        {
            _service.Salvar(movimentacao);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(movimentacao);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id)
    {
        var movimentacao = _service.ListarTodos().FirstOrDefault(m => m.Id == id);
        if (movimentacao == null)
            return NotFound();

        CarregarDadosAuxiliares();
        return View(movimentacao);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id, MovimentacaoEstoque movimentacao)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(movimentacao);
        }

        try
        {
            _service.AtualizarMovimentacaoEstoque(movimentacao, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(movimentacao);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var movimentacao = _service.ListarTodos().FirstOrDefault(m => m.Id == id);
        if (movimentacao == null)
            return NotFound();

        return View(movimentacao);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarMovimentacaoEstoque(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    private void CarregarDadosAuxiliares()
    {
        ViewBag.Produtos = new SelectList(_produtoService.ListarTodos(), "Id", "Nome");
        ViewBag.Usuarios = new SelectList(_usuarioService.ListarTodos(), "Id", "Nome");
        ViewBag.Clientes = new SelectList(_clienteService.ListarTodos(), "Id", "Nome");
    }
}