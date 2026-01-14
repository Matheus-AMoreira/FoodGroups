import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { ResumoRefeicaoDTO } from '../../models/types';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule], // Note: CommonModule pode ser removido se não usar Pipes antigos
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  protected readonly Object = Object;
  private api = inject(ApiService);

  // Estados como Signals
  agenda = signal<Record<string, ResumoRefeicaoDTO[]>>({});
  mes = signal(new Date().getMonth() + 1);
  ano = signal(new Date().getFullYear());

  // Listas geradas dinamicamente
  meses = signal(
    Array.from({ length: 12 }, (_, i) => 
      new Intl.DateTimeFormat('pt-BR', { month: 'long' }).format(new Date(2026, i))
    ).map(m => m.charAt(0).toUpperCase() + m.slice(1))
  );
  
  anos = signal(
    Array.from({ length: 2 }, (_, i) => new Date().getFullYear() + i)
  );

  // Computado para evitar Object.keys no template
  datasAgenda = computed(() => Object.keys(this.agenda()));

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    // Acessamos o valor dos signals com ()
    this.api.obterAgendaMensal(this.mes(), this.ano())
      .subscribe({
        next: (data) => {
          this.agenda.set(data);
        },
        error: (err) => console.error('Erro ao carregar agenda:', err)
      });
  }

  formatarData(dataStr: string): string {
    const date = new Date(dataStr);
    return `${date.getDate()}/${date.getMonth() + 1} (${date.toLocaleDateString('pt-BR', { weekday: 'short' })})`;
  }
}