using EstoqueCFTV.Data;
using EstoqueCFTV.Models;
using Microsoft.AspNetCore.Identity;

namespace EstoqueCFTV;

public static class SeedData
{
    public static void Inicializar(AppDbContext context, IConfiguration configuration)
    {
        string email = configuration["SeedAdmin:Email"] ?? "";
        string senha = configuration["SeedAdmin:Password"] ?? "";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            throw new InvalidOperationException("Configure o e-mail e a senha do Administrador inicial.");

        if (context.Usuarios.Any(u => u.Email == email))
            return;

        var administrador = new Usuario
        {
            Nome = "Administrador",
            Email = email,
            Perfil = "Administrador"
        };

        var hasher = new PasswordHasher<Usuario>();

        administrador.SenhaHash = hasher.HashPassword(administrador, senha);

        context.Usuarios.Add(administrador);
        context.SaveChanges();
    }
}