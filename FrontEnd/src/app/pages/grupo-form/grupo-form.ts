import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Grupo, Usuario, AgendaGrupo } from '../../models/types';
import { UserSearchComponent } from '../../components/user-search/user-search.component';

@Component({
  selector: 'app-grupo-form',
  standalone: true,
  imports: [CommonModule, FormsModule, UserSearchComponent, RouterLink],
  templateUrl: './grupo-form.html',
})
export class GrupoForm implements OnInit {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isEdit = signal(false);
  donoTemp = signal<Usuario | undefined>(undefined);
  grupo = signal<Grupo>({
    id: 0, nome: '', capacidadeMaxima: 10, criadorId: 0,
    usuarios: [], agendas: []
  });
  novaAgenda = signal<AgendaGrupo>({ refeicao: 1, ehRecorrente: true, diaSemana: 1 });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.api.obterGrupo(+id).subscribe(g => this.grupo.set(g));
    }
  }

  updateGrupo(prop: keyof Grupo, value: any) {
    this.grupo.update(g => ({ ...g, [prop]: value }));
  }

  updateNovaAgenda(prop: keyof AgendaGrupo, value: any) {
    this.novaAgenda.update(a => ({ ...a, [prop]: value }));
  }

  setDono(u: Usuario) {
    this.donoTemp.set(u);
    this.updateGrupo('criadorId', u.id);
    this.addMembro(u);
  }

  addMembro(u: Usuario) {
    this.grupo.update(g => {
      if (!g.usuarios.find(x => x.id === u.id)) {
        return { ...g, usuarios: [...g.usuarios, u] };
      }
      return g;
    });
  }

  removeMembro(u: Usuario) {
    this.grupo.update(g => ({
      ...g,
      usuarios: g.usuarios.filter(x => x.id !== u.id)
    }));
  }

  addAgenda() {
    this.grupo.update(g => ({
      ...g,
      agendas: [...g.agendas, { ...this.novaAgenda() }]
    }));
  }

  removeAgenda(index: number) {
    this.grupo.update(g => {
      const novasAgendas = [...g.agendas];
      novasAgendas.splice(index, 1);
      return { ...g, agendas: novasAgendas };
    });
  }

  salvar() {
    const dados = this.grupo();
    if (this.isEdit()) {
      this.api.atualizarGrupo(dados.id, dados).subscribe(() => this.router.navigate(['/']));
    } else {
      this.api.criarGrupo(dados).subscribe(() => this.router.navigate(['/']));
    }
  }

  getNomeRefeicao(r: number) { return ['Café', 'Almoço', 'Jantar'][r] || 'Desconhecida'; }
  getDiaSemana(d: number) { return ['Dom','Seg','Ter','Qua','Qui','Sex','Sab'][d] || 'N/A'; }
}