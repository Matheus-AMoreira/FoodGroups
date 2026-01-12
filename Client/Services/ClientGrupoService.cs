using System.Net.Http.Json;
using FoodGroups.Shared.Models;
using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Interfaces;

namespace FoodGroups.Client.Services;

public class ClientGrupoService : IGrupoService
{
    private readonly HttpClient _http;
    public ClientGrupoService(HttpClient http) => _http = http;

    // Listar (GET)
    public async Task<List<Grupo>> ListarTodos()
    {
        // Chama o endpoint GET api/grupo do Controller
        return await _http.GetFromJsonAsync<List<Grupo>>("api/grupo") ?? new List<Grupo>();
    }

    // Criar (POST)
    public async Task<String> CriarGrupo(Grupo grupo)
    {
        var response = await _http.PostAsJsonAsync("api/grupo", grupo);
        
        // Envia o objeto para o POST api/grupo
        return await response.Content.ReadAsStringAsync();
    }

    // Obter Por ID (GET)
    public async Task<Grupo?> ObterPorId(int id)
    {
        try {
            return await _http.GetFromJsonAsync<Grupo>($"api/grupo/{id}");
        } catch {
            return null;
        }
    }
    
    // Método da Agenda Mensal
    public async Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano)
    {
        // Monta a query string
        var query = $"api/grupo/resumo-mensal?mes={mes}&ano={ano}";
        return await _http.GetFromJsonAsync<Dictionary<string, List<ResumoRefeicaoDTO>>>(query) 
               ?? new Dictionary<string, List<ResumoRefeicaoDTO>>();
    }

    // Adicionar Usuário (POST)
    public async Task<String> AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId)
    {
        // No PostAsJsonAsync, o terceiro argumento pode ser headers se precisar, 
        // mas aqui vamos simplificar passando o solicitante via header manualmente se necessario,
        // ou ajustando o DTO. Para este exemplo simples:s
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/grupo/{grupoId}/adicionar-usuario");
        request.Headers.Add("solicitanteId", solicitanteId.ToString());
        request.Content = JsonContent.Create(usuarioId);

        var response = await _http.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task Atualizar(int id, Grupo grupo) => await _http.PutAsJsonAsync($"api/grupo/{id}", grupo);
    public async Task Deletar(int id) => await _http.DeleteAsync($"api/grupo/{id}");
}