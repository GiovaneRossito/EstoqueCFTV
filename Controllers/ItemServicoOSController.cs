using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class ItemServicoOSController : Controller
{
    private readonly ItemServicoOSService _service;
    private readonly OrdemDeServicoService _osService;
    private readonly ServicoService _servicoService;

    public ItemServicoOSController(
        ItemServicoOSService service,
        OrdemDeServicoService osService,
        ServicoService servicoService)
    {
        _service = service;
        _osService = osService;
        _servicoService = servicoService;
    }

    public IActionResult Index(string? busca, int? ordemId)
    {
        var itens = _service.ListarTodos().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            string termo = busca.Trim();
            itens = itens.Where(i =>
                i.Servico != null && i.Servico.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase));
        }

        if (ordemId.HasValue && ordemId.Value > 0)
            itens = itens.Where(i => i.OrdemDeServicoId == ordemId.Value);

        ViewBag.BuscaAtual = busca;
        ViewBag.OrdemAtual = ordemId;
        return View(itens.ToList());
    }

    public IActionResult Create()
    {
        CarregarDadosAuxiliares();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ItemServicoOS item)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(item);
        }

        try
        {
            _service.Salvar(item);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(item);
        }
    }

    public IActionResult Edit(int ordemDeServicoId, int servicoId)
    {
        var item = _service.ListarTodos().FirstOrDefault(i =>
            i.OrdemDeServicoId == ordemDeServicoId && i.ServicoId == servicoId);

        if (item == null)
            return NotFound();

        CarregarDadosAuxiliares();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int ordemDeServicoId, int servicoId, ItemServicoOS item)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(item);
        }

        try
        {
            _service.AtualizarItemServicoOs(item, ordemDeServicoId, servicoId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(item);
        }
    }

    public IActionResult Delete(int ordemDeServicoId, int servicoId)
    {
        var item = _service.ListarTodos().FirstOrDefault(i =>
            i.OrdemDeServicoId == ordemDeServicoId && i.ServicoId == servicoId);

        if (item == null)
            return NotFound();

        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int ordemDeServicoId, int servicoId)
    {
        try
        {
            _service.DeletarItemServicoOs(ordemDeServicoId, servicoId);
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
        ViewBag.OrdensDeServico = new SelectList(_osService.ListarTodos(), "Id", "Id");
        ViewBag.Servicos = new SelectList(_servicoService.ListarTodos(), "Id", "Nome");
    }
}