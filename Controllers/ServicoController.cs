using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class ServicoController : Controller
{
    private readonly ServicoService _service;

    public ServicoController(ServicoService service)
    {
        _service = service;
    }

    public IActionResult Index(string? busca)
    {
        var servicos = _service.ListarTodos().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            string termo = busca.Trim();
            servicos = servicos.Where(s =>
                s.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                (s.Descricao != null && s.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase)));
        }

        ViewBag.BuscaAtual = busca;
        return View(servicos.ToList());
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Create(Servico servico)
    {
        if (!ModelState.IsValid)
            return View(servico);

        try
        {
            _service.Salvar(servico);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(servico);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id)
    {
        var servico = _service.ListarTodos().FirstOrDefault(s => s.Id == id);
        if (servico == null)
            return NotFound();

        return View(servico);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id, Servico servico)
    {
        if (!ModelState.IsValid)
            return View(servico);

        try
        {
            _service.AtualizarServico(servico, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(servico);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var servico = _service.ListarTodos().FirstOrDefault(s => s.Id == id);
        if (servico == null)
            return NotFound();

        return View(servico);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarServico(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}