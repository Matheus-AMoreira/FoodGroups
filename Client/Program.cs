using FoodGroups.Shared.Interfaces;
using FoodGroups.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configura o HttpClient para mandar requisições para o endereço do Server (origin)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registra a implementação dos serviços do cliente
builder.Services.AddScoped<IGrupoService, ClientGrupoService>();
builder.Services.AddScoped<IUsuarioService, ClientUsuarioService>();

await builder.Build().RunAsync();