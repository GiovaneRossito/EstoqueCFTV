using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ClienteController
{
    private ClienteService service;

    public ClienteController(ClienteService service)
    {
        this.service = service;
    }
    
    public List<Cliente> Listar()
    {
        return service.ListarTodos();
    }

    public Cliente Salvar(Cliente cliente)
    {
        return service.Salvar(cliente);
    }

    public Cliente Atualizar(Cliente cliente, long id)
    {
        return service.AtualizarCliente(cliente, id);
    }

    public Cliente Excluir(long id)
    {
        return service.DeletarCliente(id);
    }
}