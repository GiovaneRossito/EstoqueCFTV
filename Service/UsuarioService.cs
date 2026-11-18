
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.AspNetCore.Identity;

namespace EstoqueCFTV.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();

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

        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, usuario.SenhaHash);
        
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        return usuario;
    }

    public Usuario AtualizarUsuario(Usuario usuario, int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID do usuário inválido.");

        var usuarioExistente = _context.Usuarios.Find(id);

        if (usuarioExistente == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        ValidarUsuario(usuario, id, false);

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Perfil = usuario.Perfil;
        
        if (!string.IsNullOrWhiteSpace(usuario.SenhaHash))
        {
            usuarioExistente.SenhaHash = _passwordHasher.HashPassword(usuarioExistente, usuario.SenhaHash);
        }
        
        _context.SaveChanges();

        return usuarioExistente;
    }

    public Usuario DeletarUsuario(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID do usuário inválido.");

        var usuario = _context.Usuarios.Find(id);

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

    private void ValidarUsuario(Usuario usuario, int idAtual = 0, bool exigirSenha = true)
    {
        if (exigirSenha)
        {
            if (string.IsNullOrWhiteSpace(usuario.SenhaHash))
            {
                throw new ArgumentException("A senha do usuário é obrigatória.");
            }
        }
        
        if (string.IsNullOrWhiteSpace(usuario.Nome))
            throw new ArgumentException("O nome do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(usuario.Email))
            throw new ArgumentException("O e-mail do usuário é obrigatório.");

        bool emailJaExiste = _context.Usuarios.Any(u =>
            u.Email == usuario.Email && u.Id != idAtual);

        if (emailJaExiste)
            throw new ArgumentException("Já existe um usuário com esse e-mail.");
    }

    public bool VerificarSenha(Usuario usuario, string senhaDigitada)
    {
        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senhaDigitada);
        return ( resultado != PasswordVerificationResult.Failed);
    }
}
