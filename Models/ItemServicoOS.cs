namespace EstoqueCFTV.Models;

public class ItemServicoOS 
{
    public int OrdemDeServicoId { get; set; }
    public OrdemDeServico? OrdemDeServico { get; set; }

    public int ServicoId { get; set; }
    public Servico? Servico { get; set; }

    public int Quantidade { get; set; }
    public decimal ValorCobrado { get; set; }
}
