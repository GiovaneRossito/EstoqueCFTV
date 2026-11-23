using Microsoft.EntityFrameworkCore;
using EstoqueCFTV.Data;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using EstoqueCFTV; // Necessário para encontrar a classe SeedData

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(
        // Windows: LocalDB (DefaultConnection). Linux: SQL Server em Docker (LinuxConnection).
        OperatingSystem.IsWindows() ? "DefaultConnection" : "LinuxConnection")));

builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ItemServicoOSService>();
builder.Services.AddScoped<MovimentacaoEstoqueService>();
builder.Services.AddScoped<OrdemDeServicoService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<UsuarioService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var configuration = services.GetRequiredService<IConfiguration>();
    
    // Cria o banco e as tabelas caso ainda não existam (não há Migrations no projeto)
    context.Database.EnsureCreated();

    SeedData.Inicializar(context, configuration);
}
// ==========================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();