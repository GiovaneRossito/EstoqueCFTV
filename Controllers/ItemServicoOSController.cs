using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ItemServicoOSController
{
    private ItemServicoOSService _service;

    public ItemServicoOSController(ItemServicoOSService service)
    {
        this._service = service;
    }
    
    public List<ItemServicoOS> Listar()
    {
        return _service.ListarTodos();
    }

    public ItemServicoOS Salvar(ItemServicoOS itemServicoOs)
    {
        return _service.Salvar(itemServicoOs);
    }

    public ItemServicoOS Atualizar(ItemServicoOS ItemServicoOs, int ordemDeServicoId, int servicoId)
    {
        return _service.AtualizarItemServicoOs(ItemServicoOs, ordemDeServicoId, servicoId);
    }

    public ItemServicoOS Excluir(int ordemDeServicoId, int servicoId)
    {
        return _service.DeletarItemServicoOs(ordemDeServicoId, servicoId);
    }
}