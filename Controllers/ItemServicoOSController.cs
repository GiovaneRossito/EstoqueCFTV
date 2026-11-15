using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ItemServicoOSController
{
    private ItemServicoOSService service;

    public ItemServicoOSController(ItemServicoOSService service)
    {
        this.service = service;
    }
    
    public List<ItemServicoOS> Listar()
    {
        return service.ListarTodos();
    }

    public ItemServicoOS Salvar(ItemServicoOS itemServicoOs)
    {
        return service.Salvar(itemServicoOs);
    }

    public ItemServicoOS Atualizar(ItemServicoOS ItemServicoOs, long ordemDeServicoId, int servicoId)
    {
        return service.AtualizarItemServicoOs(ItemServicoOs, ordemDeServicoId, servicoId);
    }

    public ItemServicoOS Excluir(long ordemDeServicoId, int servicoId)
    {
        return service.DeletarItemServicoOs(ordemDeServicoId, servicoId);
    }
}