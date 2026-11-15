using EstoqueCFTV.Data;
using EstoqueCFTV.Models;

namespace EstoqueCFTV.Service;

public class CategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public List<Categoria> ListarTodos()
    {
        return _context.Categorias.ToList();
    }

    
    public Categoria Salvar(Categoria categoria)
    {
        ValidarCategoria(categoria);

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }
    

    public Categoria AtualizarCategoria(Categoria categoria, long id)
    {
        if (id <= 0 || id > int.MaxValue)
        {
            throw new ArgumentException("ID da categoria inválido.");
        }

        var categoriaExistente = _context.Categorias.Find((int)id);

        if (categoriaExistente == null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        ValidarCategoria(categoria, (int)id);

        categoriaExistente.Nome = categoria.Nome;
        categoriaExistente.Descricao = categoria.Descricao;

        _context.SaveChanges();

        return categoriaExistente;
    }


    public Categoria DeletarCategoria(long id)
    {
        if (id <= 0 || id > int.MaxValue)
        {
            throw new ArgumentException("ID da categoria inválido.");
        }

        var categoria = _context.Categorias.Find((int)id);

        if (categoria == null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        if (_context.Produtos.Any(p => p.CategoriaId == id))
        {
            throw new InvalidOperationException(
                "Não é possível excluir uma categoria que possui produtos associados.");
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return categoria;
    }

    
    private void ValidarCategoria(Categoria categoria, int idAtual = 0)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
        {
            throw new ArgumentException("O nome da categoria é obrigatório.");
        }

        bool nomeJaExiste = _context.Categorias.Any(c =>
            c.Nome == categoria.Nome && c.Id != idAtual);

        if (nomeJaExiste)
        {
            throw new ArgumentException("Já existe uma categoria com esse nome.");
        }
    }
}
