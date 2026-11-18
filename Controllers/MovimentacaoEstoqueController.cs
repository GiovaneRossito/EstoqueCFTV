using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class MovimentacaoEstoqueController
{
    private MovimentacaoEstoqueService _service;

    public MovimentacaoEstoqueController(MovimentacaoEstoqueService service)
    {
        this._service = service;
    }
    
    public List<MovimentacaoEstoque> Listar()
    {
        return _service.ListarTodos();
    }

    public MovimentacaoEstoque Salvar(MovimentacaoEstoque movimentacaoEstoque)
    {
        return _service.Salvar(movimentacaoEstoque);
    }

    public MovimentacaoEstoque Atualizar(MovimentacaoEstoque movimentacaoEstoque, int id)
    {
        return _service.AtualizarMovimentacaoEstoque(movimentacaoEstoque, id);
    }

    public MovimentacaoEstoque Excluir(int id)
    {
        return _service.DeletarMovimentacaoEstoque(id);
    }
}