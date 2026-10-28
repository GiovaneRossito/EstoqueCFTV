namespace EstoqueCFTV.Models;

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
        public string? Observacao { get; set; } 
        
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
