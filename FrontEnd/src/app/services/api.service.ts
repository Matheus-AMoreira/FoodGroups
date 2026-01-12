import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Grupo, Usuario, ResumoRefeicaoDTO } from '../models/types';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5043/api'; // URL do seu Back-end ASP.NET

  // --- Usuários ---
  buscarUsuarios(termo: string): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.baseUrl}/usuario/buscar?termo=${termo}`);
  }

  listarUsuarios(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.baseUrl}/usuario`);
  }

  obterUsuario(id: number): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.baseUrl}/usuario/${id}`);
  }

  // O Backend espera um DTO para criar (com senha)
  criarUsuario(usuario: any): Observable<string> {
    return this.http.post(`${this.baseUrl}/usuario`, usuario, { responseType: 'text' });
  }

  atualizarUsuario(id: number, usuario: Usuario): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/usuario/${id}`, usuario);
  }

  deletarUsuario(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/usuario/${id}`);
  }

  // --- Grupos ---
  listarGrupos(): Observable<Grupo[]> {
    return this.http.get<Grupo[]>(`${this.baseUrl}/grupo`);
  }

  obterGrupo(id: number): Observable<Grupo> {
    return this.http.get<Grupo>(`${this.baseUrl}/grupo/${id}`);
  }

  criarGrupo(grupo: Partial<Grupo>): Observable<string> {
    return this.http.post(`${this.baseUrl}/grupo`, grupo, { responseType: 'text' });
  }

  atualizarGrupo(id: number, grupo: Grupo): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/grupo/${id}`, grupo);
  }

  // --- Dashboard ---
  obterAgendaMensal(mes: number, ano: number): Observable<Record<string, ResumoRefeicaoDTO[]>> {
    return this.http.get<Record<string, ResumoRefeicaoDTO[]>>(`${this.baseUrl}/grupo/resumo-mensal?mes=${mes}&ano=${ano}`);
  }

}