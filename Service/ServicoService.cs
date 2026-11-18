
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;

namespace EstoqueCFTV.Service;

public class ServicoService
{
    private readonly AppDbContext _context;

    public ServicoService(AppDbContext context)
    {
        _context = context;
    }

    public List<Servico> ListarTodos()
    {
        return _context.Servicos.ToList();
    }

    public Servico Salvar(Servico servico)
    {
        ValidarServico(servico);

        _context.Servicos.Add(servico);
        _context.SaveChanges();

        return servico;
    }

    public Servico AtualizarServico(Servico servico, int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID do serviço inválido.");

        var servicoExistente = _context.Servicos.Find(id);

        if (servicoExistente == null)
            throw new KeyNotFoundException("Serviço não encontrado.");

        ValidarServico(servico);

        servicoExistente.Nome = servico.Nome;
        servicoExistente.Descricao = servico.Descricao;
        servicoExistente.PrecoMaoDeObra = servico.PrecoMaoDeObra;

        _context.SaveChanges();

        return servicoExistente;
    }

    public Servico DeletarServico(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID do serviço inválido.");

        var servico = _context.Servicos.Find(id);

        if (servico == null)
            throw new KeyNotFoundException("Serviço não encontrado.");

        bool possuiOrdens = _context.ItemServicos
            .Any(item => item.ServicoId == id);

        if (possuiOrdens)
            throw new InvalidOperationException(
                "Não é possível excluir um serviço vinculado a uma ordem de serviço.");

        _context.Servicos.Remove(servico);
        _context.SaveChanges();

        return servico;
    }

    private void ValidarServico(Servico servico)
    {
        if (string.IsNullOrWhiteSpace(servico.Nome))
            throw new ArgumentException("O nome do serviço é obrigatório.");

        if (servico.PrecoMaoDeObra < 0)
            throw new ArgumentException("O preço da mão de obra não pode ser negativo.");
    }
}
