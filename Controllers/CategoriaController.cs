using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class CategoriaController
{
    private CategoriaService _service;

    public CategoriaController(CategoriaService service)
    {
        this._service = service;
    }
    
    public List<Categoria> Listar()
    {
        return _service.ListarTodos();
    }

    public Categoria Salvar(Categoria categoria)
    {
        return _service.Salvar(categoria);
    }

    public Categoria Atualizar(Categoria categoria, int id)
    {
        return _service.AtualizarCategoria(categoria, id);
    }

    public Categoria Excluir(int id)
    {
        return _service.DeletarCategoria(id);
    }
}