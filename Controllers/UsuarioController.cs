using EstoqueCFTV.Models;
using EstoqueCFTV.Service;

namespace EstoqueCFTV.Controllers;

public class UsuarioController
{
    private UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        this._service = service;
    }
    
    public List<Usuario> Listar()
    {
        return _service.ListarTodos();
    }

    public Usuario Salvar(Usuario usuario)
    {
        return _service.Salvar(usuario);
    }

    public Usuario Atualizar(Usuario usuario, int id)
    {
        return _service.AtualizarUsuario(usuario, id);
    }

    public Usuario Excluir(int id)
    {
        return _service.DeletarUsuario(id);
    }
}