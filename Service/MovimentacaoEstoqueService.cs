
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Service;

public class MovimentacaoEstoqueService
{
    private readonly AppDbContext _context;

    public MovimentacaoEstoqueService(AppDbContext context)
    {
        _context = context;
    }

    public List<MovimentacaoEstoque> ListarTodos()
    {
        return _context.Movimentacoes
            .Include(m => m.Produto)
            .Include(m => m.Usuario)
            .Include(m => m.Cliente)
            .ToList();
    }


    public MovimentacaoEstoque Salvar(MovimentacaoEstoque movimentacao)
    {
        ValidarMovimentacao(movimentacao);

        var produto = _context.Produtos.Find(movimentacao.ProdutoId);

        if (produto == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        if (movimentacao.Tipo == TipoMovimentacao.Saida &&
            produto.QuantidadeEstoque < movimentacao.Quantidade)
        {
            throw new InvalidOperationException(
                "Estoque insuficiente para realizar essa saída.");
        }

        if (movimentacao.Tipo == TipoMovimentacao.Entrada)
            produto.QuantidadeEstoque += movimentacao.Quantidade;
        else
            produto.QuantidadeEstoque -= movimentacao.Quantidade;

        _context.Movimentacoes.Add(movimentacao);
        _context.SaveChanges();

        return movimentacao;
    }


    public MovimentacaoEstoque AtualizarMovimentacaoEstoque(
        MovimentacaoEstoque movimentacao, int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da movimentação inválido.");

        var existente = _context.Movimentacoes.Find(id);

        if (existente == null)
            throw new KeyNotFoundException("Movimentação não encontrada.");

        ValidarMovimentacao(movimentacao);

        var produtoAntigo = _context.Produtos.Find(existente.ProdutoId);
        var produtoNovo = _context.Produtos.Find(movimentacao.ProdutoId);

        if (produtoAntigo == null || produtoNovo == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        int estoqueRestaurado = produtoAntigo.QuantidadeEstoque;

        if (existente.Tipo == TipoMovimentacao.Saida)
            estoqueRestaurado += existente.Quantidade;
        else
            estoqueRestaurado -= existente.Quantidade;

        if (estoqueRestaurado < 0)
            throw new InvalidOperationException(
                "Não é possível atualizar: o estoque já utilizou parte dessa entrada.");

        int estoqueDisponivel = produtoNovo.Id == produtoAntigo.Id
            ? estoqueRestaurado
            : produtoNovo.QuantidadeEstoque;

        if (movimentacao.Tipo == TipoMovimentacao.Saida &&
            estoqueDisponivel < movimentacao.Quantidade)
        {
            throw new InvalidOperationException(
                "Estoque insuficiente para realizar essa saída.");
        }

        produtoAntigo.QuantidadeEstoque = estoqueRestaurado;

        if (movimentacao.Tipo == TipoMovimentacao.Entrada)
            produtoNovo.QuantidadeEstoque += movimentacao.Quantidade;
        else
            produtoNovo.QuantidadeEstoque -= movimentacao.Quantidade;

        existente.ProdutoId = movimentacao.ProdutoId;
        existente.Tipo = movimentacao.Tipo;
        existente.Quantidade = movimentacao.Quantidade;
        existente.DataMovimentacao = movimentacao.DataMovimentacao;
        existente.Observacao = movimentacao.Observacao;
        existente.UsuarioId = movimentacao.UsuarioId;
        existente.ClienteId = movimentacao.ClienteId;

        _context.SaveChanges();

        return existente;
    }


    public MovimentacaoEstoque DeletarMovimentacaoEstoque(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da movimentação inválido.");

        var movimentacao = _context.Movimentacoes.Find(id);

        if (movimentacao == null)
            throw new KeyNotFoundException("Movimentação não encontrada.");

        var produto = _context.Produtos.Find(movimentacao.ProdutoId);

        if (produto == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        if (movimentacao.Tipo == TipoMovimentacao.Entrada)
        {
            if (produto.QuantidadeEstoque < movimentacao.Quantidade)
                throw new InvalidOperationException(
                    "Não é possível excluir essa entrada, pois parte dela já foi utilizada.");

            produto.QuantidadeEstoque -= movimentacao.Quantidade;
        }
        else
        {
            produto.QuantidadeEstoque += movimentacao.Quantidade;
        }

        _context.Movimentacoes.Remove(movimentacao);
        _context.SaveChanges();

        return movimentacao;
    }


    private void ValidarMovimentacao(MovimentacaoEstoque movimentacao)
    {
        if (movimentacao.ProdutoId <= 0)
            throw new ArgumentException("O produto é obrigatório.");

        if (movimentacao.UsuarioId <= 0)
            throw new ArgumentException("O usuário é obrigatório.");

        if (movimentacao.Quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        if (!Enum.IsDefined(typeof(TipoMovimentacao), movimentacao.Tipo))
            throw new ArgumentException("Tipo de movimentação inválido.");

        if (!_context.Usuarios.Any(u => u.Id == movimentacao.UsuarioId))
            throw new ArgumentException("Usuário não encontrado.");

        if (movimentacao.ClienteId.HasValue &&
            !_context.Clientes.Any(c => c.Id == movimentacao.ClienteId.Value))
        {
            throw new ArgumentException("Cliente não encontrado.");
        }
    }
}
