using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class UsuarioController
{
    private UsuarioService service;

    public UsuarioController(UsuarioService service)
    {
        this.service = service;
    }
    
    public List<Usuario> Listar()
    {
        return service.ListarTodos();
    }

    public Usuario Salvar(Usuario usuario)
    {
        return service.Salvar(usuario);
    }

    public Usuario Atualizar(Usuario usuario, long id)
    {
        return service.AtualizarUsuario(usuario, id);
    }

    public Usuario Excluir(long id)
    {
        return service.DeletarUsuario(id);
    }
}