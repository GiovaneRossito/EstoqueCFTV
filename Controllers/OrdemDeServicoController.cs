using EstoqueCFTV.Models;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstoqueCFTV.Controllers;

public class OrdemDeServicoController : Controller
{
    private readonly OrdemDeServicoService _service;
    private readonly ClienteService _clienteService;
    private readonly UsuarioService _usuarioService;

    public OrdemDeServicoController(
        OrdemDeServicoService service,
        ClienteService clienteService,
        UsuarioService usuarioService)
    {
        _service = service;
        _clienteService = clienteService;
        _usuarioService = usuarioService;
    }

    public IActionResult Index()
    {
        var ordens = _service.ListarTodos();
        return View(ordens);
    }

    public IActionResult Create()
    {
        CarregarDadosAuxiliares();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(OrdemDeServico ordem)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(ordem);
        }

        try
        {
            _service.Salvar(ordem);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(ordem);
        }
    }

    public IActionResult Edit(int id)
    {
        var ordem = _service.ListarTodos().FirstOrDefault(os => os.Id == id);
        if (ordem == null)
            return NotFound();

        CarregarDadosAuxiliares();
        return View(ordem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, OrdemDeServico ordem)
    {
        if (!ModelState.IsValid)
        {
            CarregarDadosAuxiliares();
            return View(ordem);
        }

        try
        {
            _service.AtualizarOrdemDeServico(ordem, id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            CarregarDadosAuxiliares();
            return View(ordem);
        }
    }

    public IActionResult Delete(int id)
    {
        var ordem = _service.ListarTodos().FirstOrDefault(os => os.Id == id);
        if (ordem == null)
            return NotFound();

        return View(ordem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _service.DeletarOrdemDeServico(id);
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
        ViewBag.Clientes = new SelectList(_clienteService.ListarTodos(), "Id", "Nome");
        ViewBag.Usuarios = new SelectList(_usuarioService.ListarTodos(), "Id", "Nome");
    }
}