
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;

namespace EstoqueCFTV.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public List<Usuario> ListarTodos()
    {
        return _context.Usuarios.ToList();
    }

    public Usuario Salvar(Usuario usuario)
    {
        ValidarUsuario(usuario);

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        return usuario;
    }

    public Usuario AtualizarUsuario(Usuario usuario, long id)
    {
        if (id <= 0 || id > int.MaxValue)
            throw new ArgumentException("ID do usuário inválido.");

        var usuarioExistente = _context.Usuarios.Find((int)id);

        if (usuarioExistente == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        ValidarUsuario(usuario, (int)id);

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.SenhaHash = usuario.SenhaHash;
        usuarioExistente.Perfil = usuario.Perfil;

        _context.SaveChanges();

        return usuarioExistente;
    }

    public Usuario DeletarUsuario(long id)
    {
        if (id <= 0 || id > int.MaxValue)
            throw new ArgumentException("ID do usuário inválido.");

        var usuario = _context.Usuarios.Find((int)id);

        if (usuario == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        bool possuiMovimentacoes = _context.Movimentacoes
            .Any(m => m.UsuarioId == id);

        if (possuiMovimentacoes)
            throw new InvalidOperationException(
                "Não é possível excluir um usuário que possui movimentações associadas.");

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return usuario;
    }

    private void ValidarUsuario(Usuario usuario, int idAtual = 0)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nome))
            throw new ArgumentException("O nome do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(usuario.Email))
            throw new ArgumentException("O e-mail do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(usuario.SenhaHash))
            throw new ArgumentException("A senha do usuário é obrigatória.");

        bool emailJaExiste = _context.Usuarios.Any(u =>
            u.Email == usuario.Email && u.Id != idAtual);

        if (emailJaExiste)
            throw new ArgumentException("Já existe um usuário com esse e-mail.");
    }
}
