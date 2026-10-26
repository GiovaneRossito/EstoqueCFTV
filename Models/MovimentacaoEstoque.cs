namespace EstoqueCFTV.Models
{
    public enum TipoMovimentacao
    {
        Entrada = 1,
        Saida = 2
    }

    public class MovimentacaoEstoque
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;

        public string? Observacao { get; set; } // ex: "Instalação da OS #102"
        public string Responsavel { get; set; } = string.Empty;
    }
}