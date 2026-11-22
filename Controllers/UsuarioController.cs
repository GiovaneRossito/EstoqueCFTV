using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuarioController : Controller
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var usuarios = _service.ListarTodos();
        return View(usuarios);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Usuario usuario)
    {
        if (!ModelState.IsValid)
            return View(usuario);

        try
        {
            _service.Salvar(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(usuario);
        }
    }

    public IActionResult Edit(int id)
    {
        var usuario = _service.ListarTodos().FirstOrDefault(u => u.Id == id);
        if (usuario == null)
            return NotFound();

        usuario.SenhaHash = string.Empty;
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Usuario usuario)
    {
        ModelState.Remove(nameof(Usuario.SenhaHash));

        if (!ModelState.IsValid)
            return View(usuario);

        try
        {
            _service.AtualizarUsuario(usuario, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(usuario);
        }
    }

    public IActionResult Delete(int id)
    {
        var usuario = _service.ListarTodos().FirstOrDefault(u => u.Id == id);
        if (usuario == null)
            return NotFound();

        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarUsuario(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}