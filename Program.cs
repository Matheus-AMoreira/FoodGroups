using FoodGroups;
using FoodGroups.Services;
using Microsoft.EntityFrameworkCore;
using FoodGroups.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Lê o arquivo .env se tiver
DotNetEnv.Env.Load();

// Swagger
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();

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


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAntiforgery();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.Run();
