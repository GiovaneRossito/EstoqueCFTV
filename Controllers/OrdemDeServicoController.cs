using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class OrdemDeServicoController
{
    private OrdemDeServicoService _service;

    public OrdemDeServicoController(OrdemDeServicoService service)
    {
        this._service = service;
    }
    
    public List<OrdemDeServico> Listar()
    {
        return _service.ListarTodos();
    }

    public OrdemDeServico Salvar(OrdemDeServico ordemDeServico)
    {
        return _service.Salvar(ordemDeServico);
    }

    public OrdemDeServico Atualizar(OrdemDeServico ordemDeServico, int id)
    {
        return _service.AtualizarOrdemDeServico(ordemDeServico, id);
    }

    public OrdemDeServico Excluir(int id)
    {
        return _service.DeletarOrdemDeServico(id);
    }
}