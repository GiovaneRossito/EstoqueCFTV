using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ProdutoController
{
    private readonly ProdutoService service;

    public ProdutoController(ProdutoService service)
    {
        this.service = service;
    }
    
    public List<Produto> Listar()
    {
        return service.ListarTodos();
    }

    public Produto Salvar(Produto produto)
    {
        return service.Salvar(produto);
    }

    public Produto Atualizar(Produto produto, long id)
    {
        return service.AtualizarProduto(produto, id);
    }

    public Produto Excluir(long id)
    {
        return service.DeletarProduto(id)
    }
}