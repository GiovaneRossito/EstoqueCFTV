namespace EstoqueCFTV.Models
{
    public class Servico
    {
        public int Id { get; set; }
        
        public string Nome { get; set; } = string.Empty; 
        
        public string? Descricao { get; set; }
        
        public decimal PrecoMaoDeObra { get; set; }

        public ICollection<ItemServicoOS> Ordens { get; set; } = new List<ItemServicoOS>();
    }
}