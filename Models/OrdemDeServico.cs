namespace EstoqueCFTV.Models;

public class OrdemDeServico
{
    public int Id { get; set; }
    public DateTime DataAbertura { get; set; } =  DateTime.Now;
    public DateTime? DataConclusao { get; set; }
    public string Status { get; set; } = "Aberta";
    public string? Observacoes { get; set; }

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    
    public ICollection<MovimentacaoEstoque> MateriaisUtilizados { get; set; } = new List<MovimentacaoEstoque>();
    public ICollection<ItemServicoOS> ServicosRealizados { get; set; } = new List<ItemServicoOS>();
}