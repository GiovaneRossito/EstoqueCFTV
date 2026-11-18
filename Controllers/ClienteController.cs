using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ClienteController
{
    private ClienteService _service;

    public ClienteController(ClienteService service)
    {
        this._service = service;
    }
    
    public List<Cliente> Listar()
    {
        return _service.ListarTodos();
    }

    public Cliente Salvar(Cliente cliente)
    {
        return _service.Salvar(cliente);
    }

    public Cliente Atualizar(Cliente cliente, int id)
    {
        return _service.AtualizarCliente(cliente, id);
    }

    public Cliente Excluir(int id)
    {
        return _service.DeletarCliente(id);
    }
}