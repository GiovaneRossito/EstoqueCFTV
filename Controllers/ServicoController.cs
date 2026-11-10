using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class ServicoController
{
    private ServicoService service;

    public ServicoController(ServicoService service)
    {
        this.service = service;
    }
    
    public List<Servico> Listar()
    {
        return service.ListarTodos();
    }

    public Servico Salvar(Servico servico)
    {
        return service.Salvar(servico);
    }

    public Servico Atualizar(Servico servico, long id)
    {
        return service.AtualizarServico(servico, id);
    }

    public Servico Excluir(long id)
    {
        return service.DeletarServico(id);
    }
}