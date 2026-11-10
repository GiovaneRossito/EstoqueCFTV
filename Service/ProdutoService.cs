using EstoqueCFTV.Data;
using EstoqueCFTV.Models;

namespace EstoqueCFTV.Service;

public class ProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext _context)
    {
        this._context =  _context;
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
        
    }

    public void ValidarProduto(Produto produto)
    {
        
    }
}