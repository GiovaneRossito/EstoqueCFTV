
using System.ComponentModel.DataAnnotations;

namespace EstoqueCFTV.Models;

public class Servico
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    [Range(0.01, 999999, ErrorMessage = "O preço da mão de obra deve ser maior que zero.")]
    public decimal PrecoMaoDeObra { get; set; }

    public ICollection<ItemServicoOS> Ordens { get; set; } = new List<ItemServicoOS>();
}
