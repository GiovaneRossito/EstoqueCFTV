using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class ClienteController : Controller
{
    private readonly ClienteService _service;

    public ClienteController(ClienteService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var clientes = _service.ListarTodos();
        return View(clientes);
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Create(Cliente cliente)
    {
        if (!ModelState.IsValid)
            return View(cliente);

        try
        {
            _service.Salvar(cliente);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(cliente);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id)
    {
        var cliente = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id, Cliente cliente)
    {
        if (!ModelState.IsValid)
            return View(cliente);

        try
        {
            _service.AtualizarCliente(cliente, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(cliente);
        }
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var cliente = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarCliente(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}