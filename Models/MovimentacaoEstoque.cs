using System.ComponentModel.DataAnnotations;
namespace EstoqueCFTV.Models;

public enum TipoMovimentacao
{
    Entrada = 1,
    Saida = 2
}

public class MovimentacaoEstoque
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um produto válido.")]
    public int ProdutoId { get; set; }

    public Produto? Produto { get; set; }

    [Range(1, 2, ErrorMessage = "Selecione Entrada ou Saída.")]
    public TipoMovimentacao Tipo { get; set; }

    [Range(1, 999999, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }

    public DateTime DataMovimentacao { get; set; } = DateTime.Now;

    [StringLength(500, ErrorMessage = "A observação deve ter no máximo 500 caracteres.")]
    public string? Observacao { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um usuário válido.")]
    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }

    public int? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }
}
