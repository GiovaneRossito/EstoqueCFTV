using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueCFTV.Controllers;

[Authorize]
public class ProdutoController : Controller
{
    private readonly ProdutoService _service;
    private readonly CategoriaService _categoriaService;

    public ProdutoController(ProdutoService service, CategoriaService categoriaService)
    {
        _service = service;
        _categoriaService = categoriaService;
    }

    // Consulta permitida para usuários autenticados
    public IActionResult Index()
    {
        var produtos = _service.ListarTodos();
        return View(produtos);
    }

    // Somente Administrador pode abrir o cadastro
    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        CarregarCategorias();
        return View();
    }

    // Somente Administrador pode cadastrar
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Produto produto)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategorias();
            return View(produto);
        }

        try
        {
            _service.Salvar(produto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarCategorias();
            return View(produto);
        }
    }

    // Somente Administrador pode abrir a edição
    [Authorize(Roles = "Administrador")]
    public IActionResult Edit(int id)
    {
        var produto = _service.ListarTodos().FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return NotFound();

        CarregarCategorias();
        return View(produto);
    }

    // Somente Administrador pode editar
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Produto produto)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategorias();
            return View(produto);
        }

        try
        {
            _service.AtualizarProduto(produto, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarCategorias();
            return View(produto);
        }
    }

    // Somente Administrador pode abrir a exclusão
    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var produto = _service.ListarTodos().FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return NotFound();

        return View(produto);
    }

    // Somente Administrador pode excluir
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarProduto(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    private void CarregarCategorias()
    {
        ViewBag.Categorias = new SelectList(_categoriaService.ListarTodos(), "Id", "Nome");
    }
}