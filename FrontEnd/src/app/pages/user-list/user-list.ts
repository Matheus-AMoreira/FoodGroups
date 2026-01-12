import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
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
  usuarios: Usuario[] = [];

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.api.listarUsuarios().subscribe(dados => this.usuarios = dados);
  }

  deletar(id: number) {
    if(confirm('Tem certeza que deseja remover este usuário?')) {
      this.api.deletarUsuario(id).subscribe(() => this.carregar());
    }
  }
}
