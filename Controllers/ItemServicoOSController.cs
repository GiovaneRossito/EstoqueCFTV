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

    public ItemServicoOS Atualizar(ItemServicoOS itemServicoOs, long id)
    {
        return service.AtualizarItemServicoOs(itemServicoOs, id);
    }

    public ItemServicoOS Excluir(long id)
    {
        return service.DeletarItemServicoOs(id);
    }
}