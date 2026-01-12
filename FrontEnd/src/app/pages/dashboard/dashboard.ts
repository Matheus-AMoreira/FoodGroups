import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, KeyValuePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { ResumoRefeicaoDTO } from '../../models/types';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  private api = inject(ApiService);
  
  mes = new Date().getMonth() + 1;
  ano = new Date().getFullYear();
  meses = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
  anos = [2025, 2026, 2027];
  
  agenda: Record<string, ResumoRefeicaoDTO[]> = {};

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.api.obterAgendaMensal(this.mes, this.ano).subscribe(data => {
      this.agenda = data;
    });
  }

  formatarData(dataStr: string): string {
    const date = new Date(dataStr);
    return `${date.getDate()}/${date.getMonth()+1} (${date.toLocaleDateString('pt-BR', { weekday: 'short' })})`;
  }
}