
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Service;

public class OrdemDeServicoService
{
    private readonly AppDbContext _context;

    public OrdemDeServicoService(AppDbContext context)
    {
        _context = context;
    }

    public List<OrdemDeServico> ListarTodos()
    {
        return _context.OrdemDeServicos
            .Include(os => os.Cliente)
            .Include(os => os.Usuario)
            .Include(os => os.ServicosRealizados)
                .ThenInclude(item => item.Servico)
            .ToList();
    }

    public OrdemDeServico Salvar(OrdemDeServico ordem)
    {
        ValidarOrdem(ordem);

        _context.OrdemDeServicos.Add(ordem);
        _context.SaveChanges();

        return ordem;
    }

    public OrdemDeServico AtualizarOrdemDeServico(
        OrdemDeServico ordem, int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da ordem de serviço inválido.");

        var existente = _context.OrdemDeServicos.Find(id);

        if (existente == null)
            throw new KeyNotFoundException("Ordem de serviço não encontrada.");

        ValidarOrdem(ordem);

        existente.DataAbertura = ordem.DataAbertura;
        existente.DataConclusao = ordem.DataConclusao;
        existente.Status = ordem.Status;
        existente.Observacoes = ordem.Observacoes;
        existente.ClienteId = ordem.ClienteId;
        existente.UsuarioId = ordem.UsuarioId;

        _context.SaveChanges();

        return existente;
    }

    public OrdemDeServico DeletarOrdemDeServico(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da ordem de serviço inválido.");

        var ordem = _context.OrdemDeServicos.Find(id);

        if (ordem == null)
            throw new KeyNotFoundException("Ordem de serviço não encontrada.");

        bool possuiServicos = _context.ItemServicos
            .Any(item => item.OrdemDeServicoId == id);

        if (possuiServicos)
            throw new InvalidOperationException(
                "Não é possível excluir uma ordem de serviço que possui serviços associados.");

        _context.OrdemDeServicos.Remove(ordem);
        _context.SaveChanges();

        return ordem;
    }

    private void ValidarOrdem(OrdemDeServico ordem)
    {
        if (ordem.ClienteId <= 0)
            throw new ArgumentException("O cliente é obrigatório.");

        if (ordem.UsuarioId <= 0)
            throw new ArgumentException("O usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(ordem.Status))
            throw new ArgumentException("O status da ordem é obrigatório.");

        if (ordem.DataConclusao.HasValue &&
            ordem.DataConclusao.Value < ordem.DataAbertura)
            throw new ArgumentException(
                "A data de conclusão não pode ser anterior à abertura.");

        if (!_context.Clientes.Any(c => c.Id == ordem.ClienteId))
            throw new ArgumentException("Cliente não encontrado.");

        if (!_context.Usuarios.Any(u => u.Id == ordem.UsuarioId))
            throw new ArgumentException("Usuário não encontrado.");
    }
}
