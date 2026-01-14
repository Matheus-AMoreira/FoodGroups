import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Usuario } from '../../models/types';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css',
})
export class UserList implements OnInit {
  private api = inject(ApiService);
  usuarios = signal<Usuario[]>([]);

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.api.listarUsuarios().subscribe(dados => {
      this.usuarios.set(dados);
    });
  }

  deletar(id: number) {
    if(confirm('Tem certeza que deseja remover este usuário?')) {
      this.api.deletarUsuario(id).subscribe(() => this.carregar());
    }
  }
}
