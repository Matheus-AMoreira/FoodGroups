import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Grupo, Usuario, AgendaGrupo } from '../../models/types';
import { UserSearchComponent } from '../../components/user-search/user-search.component';

@Component({
  selector: 'app-grupo-form',
  standalone: true,
  imports: [CommonModule, FormsModule, UserSearchComponent],
  templateUrl: './grupo-form.html',
})
export class GrupoForm implements OnInit {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isEdit = false;
  donoTemp?: Usuario;
  
  grupo: Grupo = {
    id: 0, nome: '', capacidadeMaxima: 10, criadorId: 0,
    usuarios: [], agendas: []
  };

  novaAgenda: AgendaGrupo = { refeicao: 1, ehRecorrente: true, diaSemana: 1 };

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.api.obterGrupo(+id).subscribe(g => this.grupo = g);
    }
  }

  setDono(u: Usuario) {
    this.donoTemp = u;
    this.grupo.criadorId = u.id;
    this.addMembro(u);
  }

  addMembro(u: Usuario) {
    if (!this.grupo.usuarios.find(x => x.id === u.id)) {
      this.grupo.usuarios.push(u);
    }
  }

  removeMembro(u: Usuario) {
    this.grupo.usuarios = this.grupo.usuarios.filter(x => x.id !== u.id);
  }

  addAgenda() {
    // Clone para evitar referência
    this.grupo.agendas.push({ ...this.novaAgenda });
  }

  removeAgenda(index: number) {
    this.grupo.agendas.splice(index, 1);
  }

  salvar() {
    if (this.isEdit) {
      this.api.atualizarGrupo(this.grupo.id, this.grupo).subscribe(() => this.router.navigate(['/']));
    } else {
      this.api.criarGrupo(this.grupo).subscribe(() => this.router.navigate(['/']));
    }
  }

  // Helpers para display
  getNomeRefeicao(r: any) { return ['Café', 'Almoço', 'Jantar'][r] || r; }
  getDiaSemana(d: any) { return ['Dom','Seg','Ter','Qua','Qui','Sex','Sab'][d] || d; }
}