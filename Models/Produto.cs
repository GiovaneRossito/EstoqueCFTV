namespace EstoqueCFTV.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty; // ex: CAM-INT-1080P, CABO-UTP
        public string? Descricao { get; set; }

        public decimal PrecoUnitario { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int QuantidadeMinima { get; set; } // Para alerta de estoque baixo
        public string UnidadeMedida { get; set; } = "UN"; // "UN", "M", "CX"

        // Chave Estrangeira ligando ao id da Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}