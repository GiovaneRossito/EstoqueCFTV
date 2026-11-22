using System.ComponentModel.DataAnnotations;

namespace EstoqueCFTV.Models;

public class Cliente
{
    public  int Id { get; set; }

    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O documento do cliente é obrigatório.")]
    [StringLength(20, ErrorMessage = "O documento deve ter no máximo 20 caracteres.")]
    public string Documento { get; set; } = string.Empty;

    [RegularExpression(
    @"^\(?[1-9]{2}\)?\s?(?:9\d{4}-?\d{4}|[2-5]\d{3}-?\d{4})$",
    ErrorMessage = "Informe um telefone válido com DDD (ex.: (18) 99999-9999).")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string? Telefone { get; set; }

    [StringLength(250, ErrorMessage = "O endereço deve ter no máximo 250 caracteres.")]
    public string? Endereco { get; set; }
    
    public ICollection<MovimentacaoEstoque> Movimentacoes  { get; set; } = new List<MovimentacaoEstoque>();
}