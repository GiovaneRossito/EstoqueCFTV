using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ProdutoController
{
    private ProdutoService service;

    public ProdutoController(ProdutoService service)
    {
        this.service = service;
    }
    
    public List<Produto> Listar()
    {
        return service.ListarTodos();
    }


    
}