using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueCFTV.Controllers;

public class ServicoController : Controller
{
    private readonly ServicoService _service;

    public ServicoController(ServicoService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var servicos = _service.ListarTodos();
        return View(servicos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

    public IActionResult Edit(int id)
    {
        var servico = _service.ListarTodos().FirstOrDefault(s => s.Id == id);
        if (servico == null)
            return NotFound();

        return View(servico);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

    public IActionResult Delete(int id)
    {
        var servico = _service.ListarTodos().FirstOrDefault(s => s.Id == id);
        if (servico == null)
            return NotFound();

        return View(servico);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
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