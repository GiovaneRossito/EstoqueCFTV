namespace EstoqueCFTV.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        // Relacionamento: Uma categoria pode ter vários produtos
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}