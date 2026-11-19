using System.ComponentModel.DataAnnotations;
namespace EstoqueCFTV.Models;

public class OrdemDeServico
{
    public int Id { get; set; }

    public DateTime DataAbertura { get; set; } = DateTime.Now;

    public DateTime? DataConclusao { get; set; }

    [Required(ErrorMessage = "O status da ordem é obrigatório.")]
    [StringLength(30, ErrorMessage = "O status deve ter no máximo 30 caracteres.")]
    public string Status { get; set; } = "Aberta";

    [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres.")]
    public string? Observacoes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido.")]
    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um usuário responsável válido.")]
    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }

    public ICollection<MovimentacaoEstoque> MateriaisUtilizados { get; set; } = new List<MovimentacaoEstoque>();

    public ICollection<ItemServicoOS> ServicosRealizados { get; set; } = new List<ItemServicoOS>();
}
