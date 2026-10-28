namespace EstoqueCFTV.Models;

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        public decimal PrecoUnitario { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int QuantidadeMinima { get; set; } 
        public string UnidadeMedida { get; set; } = "UN";
        
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
