using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ProdutoController
{
    private readonly ProdutoService _service;

    public ProdutoController(ProdutoService service)
    {
        this._service = service;
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