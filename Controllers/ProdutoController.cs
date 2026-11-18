using System.Reflection.Metadata.Ecma335;
using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EstoqueCFTV.Controllers;

public class ProdutoController : Controller
{
    private readonly ProdutoService _service;
    private readonly CategoriaService _categoriaService;

    public ProdutoController(ProdutoService service, CategoriaService categoriaService)
    {
        this._service = service;
        this._categoriaService = categoriaService;
    }
    
    public IActionResult Index()
    {
        var produto = _service.ListarTodos();
        return View(produto);
    }


    public IActionResult Create()
    {
        CarregarCategorias();
        return View();
    }

    [HttpPost]
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

    public IActionResult Edit(int id)
    {
        var produto = _service.ListarTodos()
            .FirstOrDefault(p => p.Id == id);

        if (produto == null)
            return NotFound();

        CarregarCategorias();
        return View(produto);
    }

    [HttpPost]
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
    
    public IActionResult Delete(int id)
    {
        var produto = _service.ListarTodos()
            .FirstOrDefault(p => p.Id == id);

        if (produto == null)
            return NotFound();

        return View(produto);
    }

    [HttpPost, ActionName("Delete")]
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
        ViewBag.Categorias = new SelectList(
            _categoriaService.ListarTodos(),
            "Id",
            "Nome"
            );
        }

    public List<Produto> Listar()
    {
        return _service.ListarTodos();
    }

    public Produto Salvar(Produto produto)
    {
        return _service.Salvar(produto);
    }

    public Produto Atualizar(Produto produto, int id)
    {
        return _service.AtualizarProduto(produto, id);
    }

    public Produto Excluir(int id)
    {
        return _service.DeletarProduto(id);
    }
}