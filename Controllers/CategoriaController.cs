using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueCFTV.Controllers;

public class CategoriaController : Controller
{
    private readonly CategoriaService _service;

    public CategoriaController(CategoriaService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var categorias = _service.ListarTodos();
        return View(categorias);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Categoria categoria)
    {
        if (!ModelState.IsValid)
            return View(categoria);

        try
        {
            _service.Salvar(categoria);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(categoria);
        }
    }

    public IActionResult Edit(int id)
    {
        var categoria = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Categoria categoria)
    {
        if (!ModelState.IsValid)
            return View(categoria);

        try
        {
            _service.AtualizarCategoria(categoria, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(categoria);
        }
    }

    public IActionResult Delete(int id)
    {
        var categoria = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarCategoria(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}