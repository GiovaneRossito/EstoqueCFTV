using System.ComponentModel.DataAnnotations;
namespace EstoqueCFTV.Models;

public class ItemServicoOS
{
    [Range(1, int.MaxValue, ErrorMessage = "A Ordem de Serviço deve ser válida.")]
    public int OrdemDeServicoId { get; set; }

    public OrdemDeServico? OrdemDeServico { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
    public int ServicoId { get; set; }

    public Servico? Servico { get; set; }

    [Range(1, 9999, ErrorMessage = "A quantidade deve ser de pelo menos 1.")]
    public int Quantidade { get; set; }

    [Range(0.01, 999999, ErrorMessage = "O valor cobrado deve ser maior que zero.")]
    public decimal ValorCobrado { get; set; }
}

