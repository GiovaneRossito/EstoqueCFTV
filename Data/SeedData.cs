using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.AspNetCore.Identity;

namespace EstoqueCFTV;

public static class SeedData
{
    public static void Inicializar(AppDbContext context, IConfiguration configuration)
    {
        // Usa o que estiver no appsettings / User Secrets ou assume o padrão combinado para a apresentação
        string email = configuration["SeedAdmin:Email"] ?? "admin@estoque.com";
        string senha = configuration["SeedAdmin:Password"] ?? "123";

        var hasher = new PasswordHasher<Usuario>();

        var admin = context.Usuarios.FirstOrDefault(u => u.Email == email);
        if (admin == null)
        {
            admin = new Usuario
            {
                Nome = "Administrador",
                Email = email,
                Perfil = "Administrador"
            };
            admin.SenhaHash = hasher.HashPassword(admin, senha);
            context.Usuarios.Add(admin);
            context.SaveChanges();
        }

        // Usuário secundário para demonstrar Autorização por Perfil (bloqueio / Acesso Negado)
        var tecnico = context.Usuarios.FirstOrDefault(u => u.Email == "user@estoque.com");
        if (tecnico == null)
        {
            tecnico = new Usuario
            {
                Nome = "Carlos Técnico",
                Email = "user@estoque.com",
                Perfil = "Tecnico"
            };
            tecnico.SenhaHash = hasher.HashPassword(tecnico, "123");
            context.Usuarios.Add(tecnico);
            context.SaveChanges();
        }

        // Evita duplicar dados caso as tabelas já tenham sido populadas
        if (context.Categorias.Any())
            return;

        var catCameras = new Categoria { Nome = "Câmeras de Segurança", Descricao = "Câmeras Bullet, Dome e IP" };
        var catGravadores = new Categoria { Nome = "Gravadores DVR / NVR", Descricao = "Gravadores digitais e de rede" };
        var catCabos = new Categoria { Nome = "Cabos e Conectores", Descricao = "Cabos coaxiais, UTP, BNC e P4" };
        var catFontes = new Categoria { Nome = "Fontes e Energia", Descricao = "Fontes 12V e nobreaks" };

        context.Categorias.AddRange(catCameras, catGravadores, catCabos, catFontes);
        context.SaveChanges();

        var p1 = new Produto { Nome = "Câmera Bullet Full HD 1080p", SKU = "CAM-BUL-1080", Descricao = "Infravermelho 30m IP66", PrecoUnitario = 149.90m, QuantidadeEstoque = 25, QuantidadeMinima = 10, UnidadeMedida = "UN", CategoriaId = catCameras.Id };
        var p2 = new Produto { Nome = "Câmera Dome IP PoE 4MP", SKU = "CAM-DOM-4MP", Descricao = "Visão noturna inteligente", PrecoUnitario = 320.00m, QuantidadeEstoque = 4, QuantidadeMinima = 6, UnidadeMedida = "UN", CategoriaId = catCameras.Id };
        var p3 = new Produto { Nome = "DVR Multi HD 8 Canais", SKU = "DVR-08CH-HD", Descricao = "Suporta HD até 10TB", PrecoUnitario = 580.00m, QuantidadeEstoque = 7, QuantidadeMinima = 3, UnidadeMedida = "UN", CategoriaId = catGravadores.Id };
        var p4 = new Produto { Nome = "NVR IP 16 Canais 4K", SKU = "NVR-16CH-4K", Descricao = "16 portas PoE integradas", PrecoUnitario = 1450.00m, QuantidadeEstoque = 2, QuantidadeMinima = 2, UnidadeMedida = "UN", CategoriaId = catGravadores.Id };
        var p5 = new Produto { Nome = "Cabo Coaxial Bipolar 100m", SKU = "CAB-COAX-100", Descricao = "80% malha de cobre", PrecoUnitario = 115.00m, QuantidadeEstoque = 14, QuantidadeMinima = 5, UnidadeMedida = "CX", CategoriaId = catCabos.Id };
        var p6 = new Produto { Nome = "Conector BNC Mola Parafuso", SKU = "CON-BNC-01", Descricao = "Conector metálico", PrecoUnitario = 3.50m, QuantidadeEstoque = 18, QuantidadeMinima = 30, UnidadeMedida = "UN", CategoriaId = catCabos.Id };
        var p7 = new Produto { Nome = "Fonte Colmeia 12V 10A", SKU = "FNT-12V-10A", Descricao = "Chaveada para até 8 câmeras", PrecoUnitario = 68.00m, QuantidadeEstoque = 12, QuantidadeMinima = 5, UnidadeMedida = "UN", CategoriaId = catFontes.Id };

        context.Produtos.AddRange(p1, p2, p3, p4, p5, p6, p7);
        context.SaveChanges();

        var cli1 = new Cliente { Nome = "Supermercado Central Assis", Documento = "12.345.678/0001-90", Telefone = "(18) 3322-1010", Endereco = "Av. Rui Barbosa, 1200" };
        var cli2 = new Cliente { Nome = "Condomínio Residencial Jardins", Documento = "98.765.432/0001-10", Telefone = "(18) 99711-2233", Endereco = "Rua Floriano Peixoto, 450" };
        var cli3 = new Cliente { Nome = "João Marcos Almeida", Documento = "345.678.912-00", Telefone = "(18) 99644-8899", Endereco = "Rua Brasil, 890" };

        context.Clientes.AddRange(cli1, cli2, cli3);
        context.SaveChanges();

        var srv1 = new Servico { Nome = "Instalação de Câmera (Ponto)", Descricao = "Passagem de cabo e fixação", PrecoMaoDeObra = 120.00m };
        var srv2 = new Servico { Nome = "Configuração de DVR/NVR e Rede", Descricao = "Acesso remoto e gravação", PrecoMaoDeObra = 180.00m };
        var srv3 = new Servico { Nome = "Manutenção Preventiva", Descricao = "Revisão de conectores e fontes", PrecoMaoDeObra = 150.00m };

        context.Servicos.AddRange(srv1, srv2, srv3);
        context.SaveChanges();

        var os1 = new OrdemDeServico
        {
            ClienteId = cli1.Id,
            UsuarioId = tecnico.Id,
            DataAbertura = DateTime.Now.AddDays(-5),
            DataConclusao = DateTime.Now.AddDays(-3),
            Status = "Concluída",
            Observacoes = "Instalação de 4 câmeras e configuração do DVR"
        };

        var os2 = new OrdemDeServico
        {
            ClienteId = cli2.Id,
            UsuarioId = admin.Id,
            DataAbertura = DateTime.Now.AddDays(-1),
            Status = "Em andamento",
            Observacoes = "Ampliação das câmeras da portaria"
        };

        context.OrdemDeServicos.AddRange(os1, os2);
        context.SaveChanges();

        context.ItemServicos.AddRange(
            new ItemServicoOS { OrdemDeServicoId = os1.Id, ServicoId = srv1.Id, Quantidade = 4, ValorCobrado = 120.00m },
            new ItemServicoOS { OrdemDeServicoId = os1.Id, ServicoId = srv2.Id, Quantidade = 1, ValorCobrado = 180.00m },
            new ItemServicoOS { OrdemDeServicoId = os2.Id, ServicoId = srv1.Id, Quantidade = 2, ValorCobrado = 120.00m }
        );
        context.SaveChanges();

        var mov1 = new MovimentacaoEstoque { ProdutoId = p1.Id, Tipo = TipoMovimentacao.Entrada, Quantidade = 30, DataMovimentacao = DateTime.Now.AddDays(-10), UsuarioId = admin.Id, Observacao = "Compra fornecedor" };
        var mov2 = new MovimentacaoEstoque { ProdutoId = p1.Id, Tipo = TipoMovimentacao.Saida, Quantidade = 4, DataMovimentacao = DateTime.Now.AddDays(-4), UsuarioId = tecnico.Id, ClienteId = cli1.Id, Observacao = "Utilizado na OS #1" };
        var mov3 = new MovimentacaoEstoque { ProdutoId = p3.Id, Tipo = TipoMovimentacao.Entrada, Quantidade = 8, DataMovimentacao = DateTime.Now.AddDays(-10), UsuarioId = admin.Id, Observacao = "Reposição de estoque" };
        var mov4 = new MovimentacaoEstoque { ProdutoId = p3.Id, Tipo = TipoMovimentacao.Saida, Quantidade = 1, DataMovimentacao = DateTime.Now.AddDays(-4), UsuarioId = tecnico.Id, ClienteId = cli1.Id, Observacao = "Utilizado na OS #1" };
        var mov5 = new MovimentacaoEstoque { ProdutoId = p2.Id, Tipo = TipoMovimentacao.Saida, Quantidade = 2, DataMovimentacao = DateTime.Now.AddDays(-1), UsuarioId = admin.Id, ClienteId = cli2.Id, Observacao = "Saída parcial OS #2" };

        os1.MateriaisUtilizados = new List<MovimentacaoEstoque> { mov2, mov4 };
        os2.MateriaisUtilizados = new List<MovimentacaoEstoque> { mov5 };

        context.Movimentacoes.AddRange(mov1, mov2, mov3, mov4, mov5);
        context.SaveChanges();
    }
}