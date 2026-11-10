using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class CategoriaController
{
    private CategoriaService service;

    public CategoriaController(CategoriaService service)
    {
        this.service = service;
    }
    
    public List<Categoria> Listar()
    {
        return service.ListarTodos();
    }

    public Categoria Salvar(Categoria categoria)
    {
        return service.Salvar(categoria);
    }

    public Categoria Atualizar(Categoria categoria, long id)
    {
        return service.AtualizarCategoria(categoria, id);
    }

    public Categoria Excluir(long id)
    {
        return service.DeletarCategoria(id);
    }
}