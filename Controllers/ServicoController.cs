using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ServicoController
{
    private ServicoService _service;

    public ServicoController(ServicoService service)
    {
        this._service = service;
    }
    
    public List<Servico> Listar()
    {
        return _service.ListarTodos();
    }

    public Servico Salvar(Servico servico)
    {
        return _service.Salvar(servico);
    }

    public Servico Atualizar(Servico servico, int id)
    {
        return _service.AtualizarServico(servico, id);
    }

    public Servico Excluir(int id)
    {
        return _service.DeletarServico(id);
    }
}