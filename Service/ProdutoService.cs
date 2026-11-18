
using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueCFTV.Service;

public class ProdutoService
{
    
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    
    public Produto Salvar(Produto produto)
    {
        ValidarProduto(produto);

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return produto;
    }


    public List<Produto> ListarTodos()
    {
        return _context.Produtos
            .Include(p => p.Categoria)
            .ToList();
    }


    public Produto AtualizarProduto(Produto produto, int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID do produto inválido.");
        }

        var produtoExistente = _context.Produtos.Find(id);

        if (produtoExistente == null)
        {
            throw new KeyNotFoundException("Produto não encontrado.");
        }

        ValidarProduto(produto, id);

        produtoExistente.Nome = produto.Nome;
        produtoExistente.SKU = produto.SKU;
        produtoExistente.Descricao = produto.Descricao;
        produtoExistente.PrecoUnitario = produto.PrecoUnitario;
        produtoExistente.QuantidadeEstoque = produto.QuantidadeEstoque;
        produtoExistente.QuantidadeMinima = produto.QuantidadeMinima;
        produtoExistente.UnidadeMedida = produto.UnidadeMedida;
        produtoExistente.CategoriaId = produto.CategoriaId;

        _context.SaveChanges();

        return produtoExistente;
    }

    
    public Produto DeletarProduto(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID do produto inválido.");
        }

        var produto = _context.Produtos.Find(id);

        if (produto == null)
        {
            throw new KeyNotFoundException("Produto não encontrado.");
        }

        if (_context.Movimentacoes.Any(m => m.ProdutoId == id))
        {
            throw new InvalidOperationException(
                "Não é possível excluir um produto que possui movimentação de estoque.");
        }
        
        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return produto;
    }

    
    public void ValidarProduto(Produto produto, int idAtual = 0)
    {
        if (string.IsNullOrWhiteSpace(produto.Nome))
        {
            throw new ArgumentException("O nome do produto é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(produto.SKU))
        {
            throw new ArgumentException("O SKU do produto é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(produto.UnidadeMedida))
        {
            throw new ArgumentException("A unidade de medida é obrigatória.");
        }

        if (produto.PrecoUnitario < 0)
        {
            throw new ArgumentException("O preço não pode ser negativo.");
        }

        if (produto.QuantidadeEstoque < 0)
        {
            throw new ArgumentException("A quantidade em estoque não pode ser negativa.");
        }

        if (produto.QuantidadeMinima < 0)
        {
            throw new ArgumentException("A quantidade mínima não pode ser negativa.");
        }

        bool skuJaExiste = _context.Produtos.Any(p =>
            p.SKU == produto.SKU && p.Id != idAtual);

        if (skuJaExiste)
        {
            throw new ArgumentException("Já existe um produto cadastrado com esse SKU.");
        }

        bool categoriaExiste = _context.Categorias
            .Any(c => c.Id == produto.CategoriaId);

        if (!categoriaExiste)
        {
            throw new ArgumentException("A categoria informada não existe.");
        }
    }
}
