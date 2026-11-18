
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Service;

public class ItemServicoOSService
{
    private readonly AppDbContext _context;

    public ItemServicoOSService(AppDbContext context)
    {
        _context = context;
    }

    public List<ItemServicoOS> ListarTodos()
    {
        return _context.ItemServicos
            .Include(item => item.OrdemDeServico)
            .Include(item => item.Servico)
            .ToList();
    }

    public ItemServicoOS Salvar(ItemServicoOS item)
    {
        ValidarItem(item);

        var existente = _context.ItemServicos.Find(
            item.OrdemDeServicoId, item.ServicoId);

        if (existente != null)
            throw new InvalidOperationException(
                "Esse serviço já está cadastrado nessa ordem de serviço.");

        _context.ItemServicos.Add(item);
        _context.SaveChanges();

        return item;
    }

    public ItemServicoOS AtualizarItemServicoOs(
        ItemServicoOS item, int ordemDeServicoId, int servicoId)
    {
        if (ordemDeServicoId <= 0)
            throw new ArgumentException("ID da ordem de serviço inválido.");

        var existente = _context.ItemServicos.Find(
            ordemDeServicoId, servicoId);

        if (existente == null)
            throw new KeyNotFoundException("Item da ordem de serviço não encontrado.");

        ValidarItem(item);

        existente.Quantidade = item.Quantidade;
        existente.ValorCobrado = item.ValorCobrado;

        _context.SaveChanges();

        return existente;
    }

    public ItemServicoOS DeletarItemServicoOs(
        int ordemDeServicoId, int servicoId)
    {
        if (ordemDeServicoId <= 0)
            throw new ArgumentException("ID da ordem de serviço inválido.");

        var item = _context.ItemServicos.Find(
            ordemDeServicoId, servicoId);

        if (item == null)
            throw new KeyNotFoundException("Item da ordem de serviço não encontrado.");

        _context.ItemServicos.Remove(item);
        _context.SaveChanges();

        return item;
    }

    private void ValidarItem(ItemServicoOS item)
    {
        if (item.OrdemDeServicoId <= 0)
            throw new ArgumentException("A ordem de serviço é obrigatória.");

        if (item.ServicoId <= 0)
            throw new ArgumentException("O serviço é obrigatório.");

        if (item.Quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        if (item.ValorCobrado < 0)
            throw new ArgumentException("O valor cobrado não pode ser negativo.");

        if (!_context.OrdemDeServicos.Any(os => os.Id == item.OrdemDeServicoId))
            throw new ArgumentException("Ordem de serviço não encontrada.");

        if (!_context.Servicos.Any(s => s.Id == item.ServicoId))
            throw new ArgumentException("Serviço não encontrado.");
    }
}
