using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class OrdemDeServicoController
{
    private OrdemDeServicoService service;

    public OrdemDeServicoController(OrdemDeServicoService service)
    {
        this.service = service;
    }
    
    public List<OrdemDeServico> Listar()
    {
        return service.ListarTodos();
    }

    public OrdemDeServico Salvar(OrdemDeServico ordemDeServico)
    {
        return service.Salvar(ordemDeServico);
    }

    public OrdemDeServico Atualizar(OrdemDeServico ordemDeServico, long id)
    {
        return service.AtualizarOrdemDeServico(ordemDeServico, id);
    }

    public OrdemDeServico Excluir(long id)
    {
        return service.DeletarOrdemDeServico(id);
    }
}