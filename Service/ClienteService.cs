
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;

namespace EstoqueCFTV.Service;

public class ClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    
    public List<Cliente> ListarTodos()
    {
        return _context.Clientes.ToList();
    }

    
    public Cliente Salvar(Cliente cliente)
    {
        ValidarCliente(cliente);

        _context.Clientes.Add(cliente);
        _context.SaveChanges();

        return cliente;
    }

    
    public Cliente AtualizarCliente(Cliente cliente, long id)
    {
        if (id <= 0 || id > int.MaxValue)
        {
            throw new ArgumentException("ID do cliente inválido.");
        }

        var clienteExistente = _context.Clientes.Find((int)id);

        if (clienteExistente == null)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        ValidarCliente(cliente, (int)id);

        clienteExistente.Nome = cliente.Nome;
        clienteExistente.Documento = cliente.Documento;
        clienteExistente.Telefone = cliente.Telefone;
        clienteExistente.Endereco = cliente.Endereco;

        _context.SaveChanges();

        return clienteExistente;
    }

    
    public Cliente DeletarCliente(long id)
    {
        if (id <= 0 || id > int.MaxValue)
        {
            throw new ArgumentException("ID do cliente inválido.");
        }

        var cliente = _context.Clientes.Find((int)id);

        if (cliente == null)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        bool possuiMovimentacoes = _context.Movimentacoes
            .Any(m => m.ClienteId == id);

        if (possuiMovimentacoes)
        {
            throw new InvalidOperationException(
                "Não é possível excluir um cliente que possui movimentações associadas.");
        }

        _context.Clientes.Remove(cliente);
        _context.SaveChanges();

        return cliente;
    }

    
    private void ValidarCliente(Cliente cliente, int idAtual = 0)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nome))
        {
            throw new ArgumentException("O nome do cliente é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(cliente.Documento))
        {
            throw new ArgumentException("O documento do cliente é obrigatório.");
        }

        bool documentoJaExiste = _context.Clientes.Any(c =>
            c.Documento == cliente.Documento && c.Id != idAtual);

        if (documentoJaExiste)
        {
            throw new ArgumentException(
                "Já existe um cliente cadastrado com esse documento.");
        }
    }
}
