using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class CategoriaController : Controller
{
    private readonly CategoriaService _service;

    public CategoriaController(CategoriaService service)
    {
        _service = service;
    }

    // Consulta permitida para usuários autenticados
    public IActionResult Index(string? busca)
    {
        var categorias = _service.ListarTodos().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            string termo = busca.Trim();
            categorias = categorias.Where(c =>
                c.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                (c.Descricao != null && c.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase)));
        }

        ViewBag.BuscaAtual = busca;
        return View(categorias.ToList());
    }

    // Somente Administrador pode abrir o cadastro
    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        return View();
    }

    // Somente Administrador pode cadastrar
    [HttpPost]
    [Authorize(Roles = "Administrador")]
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

    // Somente Administrador pode abrir a edição
    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id)
    {
        var categoria = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    // Somente Administrador pode editar
    [HttpPost]
    [Authorize(Roles = "Administrador")]
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

    // Somente Administrador pode abrir a exclusão
    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var categoria = _service.ListarTodos().FirstOrDefault(c => c.Id == id);
        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    // Somente Administrador pode excluir
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrador")]
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