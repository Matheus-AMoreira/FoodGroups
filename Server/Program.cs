using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FoodGroups.Client;
using FoodGroups.Shared.Interfaces;
using FoodGroups.Components;
using FoodGroups.Services;

var builder = WebApplication.CreateBuilder(args);

// Lê o arquivo .env se tiver
DotNetEnv.Env.Load();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Swagger
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });;

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//  Banco de dados
var connectionString = DotNetEnv.Env.GetString(
    "DB_CONNECTION_STRING",
    builder.Configuration.GetConnectionString("DefaultConnection")
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IGrupoService, GrupoService>();

// Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGrupoRepository, GrupoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Database Connection test
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Tenta conectar. Retorna true se conseguir, ou lança erro se falhar.
    bool canConnect = await context.Database.CanConnectAsync();
    Console.WriteLine(canConnect ? "Conexão com Postgres OK!" : "Falha na conexão.");
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    Console.WriteLine("Swagger: http://localhost:5043/swagger/index.html");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthorization();

app.UseStaticFiles();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FoodGroups.Client._Imports).Assembly);

app.MapControllers();

app.Run();
