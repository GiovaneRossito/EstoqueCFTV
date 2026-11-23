using Microsoft.EntityFrameworkCore;
using EstoqueCFTV.Data;
using EstoqueCFTV.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using EstoqueCFTV; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(
        OperatingSystem.IsWindows() ? "DefaultConnection" : "LinuxConnection")));

builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ItemServicoOSService>();
builder.Services.AddScoped<MovimentacaoEstoqueService>();
builder.Services.AddScoped<OrdemDeServicoService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<UsuarioService>();

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
    
    context.Database.EnsureCreated();

    SeedData.Inicializar(context, configuration);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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