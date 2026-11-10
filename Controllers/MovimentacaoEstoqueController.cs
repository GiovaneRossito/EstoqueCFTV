using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class MovimentacaoEstoqueController
{
    private MovimentacaoEstoqueService service;

    public MovimentacaoEstoqueController(MovimentacaoEstoqueService service)
    {
        this.service = service;
    }
    
    public List<MovimentacaoEstoque> Listar()
    {
        return service.ListarTodos();
    }

    public MovimentacaoEstoque Salvar(MovimentacaoEstoque movimentacaoEstoque)
    {
        return service.Salvar(movimentacaoEstoque);
    }

    public MovimentacaoEstoque Atualizar(MovimentacaoEstoque movimentacaoEstoque, long id)
    {
        return service.AtualizarMovimentacaoEstoque(movimentacaoEstoque, id);
    }

    public MovimentacaoEstoque Excluir(long id)
    {
        return service.DeletarMovimentacaoEstoque(id);
    }
}