using System.ComponentModel.DataAnnotations;

namespace EstoqueCFTV.Models;

public class Categoria 
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome da categoria é obrigatorio.")]
    [StringLength(100, ErrorMessage = "O nome pode conter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "A descrição pode conter até 500 caracteres.")]
    public string? Descricao { get; set; }
        
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
