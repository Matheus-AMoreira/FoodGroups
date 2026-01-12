using System.Net.Http.Json;
using FoodGroups.Shared.Models;
using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Interfaces;

namespace FoodGroups.Client.Services;

public class ClientUsuarioService : IUsuarioService
{
    private readonly HttpClient _http;
    public ClientUsuarioService(HttpClient http) => _http = http;

    public async Task<List<Usuario>> ListarTodos()
    {
        return await _http.GetFromJsonAsync<List<Usuario>>("api/usuario") ?? new List<Usuario>();
    }

    public async Task<String> CriarUsuario(Usuario usuario)
    {
        // Atenção: O Controller espera um DTO ou o Modelo?
        // Se o Controller espera CriarUsuarioDTO, você deve converter aqui ou ajustar o Controller.
        // Vamos assumir que ajustamos o Controller para aceitar o Modelo ou convertemos aqui:
        
        var dto = new CriarUsuarioDTO 
        { 
            Nome = usuario.Nome, 
            Email = usuario.Email, 
            Senha = usuario.Senha 
        };

        var response = await _http.PostAsJsonAsync("api/usuario", dto);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<Usuario>> BuscarUsuarios(string termo)
    {
        return await _http.GetFromJsonAsync<List<Usuario>>($"api/usuario/buscar?termo={termo}") 
               ?? new List<Usuario>();
    }

    // Implementar os demais métodos (ObterPorId, Atualizar, Deletar) via HTTP...
    public async Task<Usuario?> ObterPorId(int id) => await _http.GetFromJsonAsync<Usuario>($"api/usuario/{id}");
    public async Task Atualizar(int id, Usuario u) => await _http.PutAsJsonAsync($"api/usuario/{id}", u);
    public async Task Deletar(int id) => await _http.DeleteAsync($"api/usuario/{id}");
}