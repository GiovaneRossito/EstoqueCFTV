using System.ComponentModel.DataAnnotations;

namespace EstoqueCFTV.Models;

    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do Produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O SKU é obrigatório.")]
        public string SKU { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição pode conter até 500 caracteres.")]
        public string? Descricao { get; set; }
        
        [Range(0.01, 999999, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
        public int QuantidadeEstoque { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade mínima não pode ser negativa.")]
        public int QuantidadeMinima { get; set; } 

        [Required(ErrorMessage = "A unidade de medida é obrigatória.")]
        public string UnidadeMedida { get; set; } = "UN";
        
        [Required(ErrorMessage = "Selecione uma categoria.")]
        public int CategoriaId { get; set; }
        
        public Categoria? Categoria { get; set; }
    }
