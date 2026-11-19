
using System.ComponentModel.DataAnnotations;

namespace EstoqueCFTV.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatório.")]
    public string SenhaHash { get; set; } = string.Empty;

    [Required(ErrorMessage = "O perfil do usuário é obrigatório.")]
    [RegularExpression("^(Administrador|Tecnico)$", ErrorMessage = "O perfil deve ser Administrador ou Tecnico.")]
    public string Perfil { get; set; } = "Tecnico";

    public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
}
