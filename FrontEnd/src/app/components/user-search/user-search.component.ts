import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Usuario } from '../../models/types';

@Component({
  selector: 'app-user-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="relative w-full">
      <div class="flex gap-2 mb-2">
        <input type="text" class="form-control" [(ngModel)]="termo" (keyup.enter)="pesquisar()" placeholder="Buscar usuário..." />
        <button class="btn btn-secondary h-[42px]" (click)="pesquisar()">Buscar</button>
      </div>

      <ul *ngIf="usuarios.length > 0" class="absolute z-10 w-full bg-white border border-gray-200 rounded shadow-lg">
        <li *ngFor="let u of usuarios" class="p-3 border-b flex justify-between items-center hover:bg-gray-50">
          <span>{{ u.nome }} <small class="text-gray-500">({{ u.email }})</small></span>
          <button class="btn btn-primary text-sm" (click)="selecionar(u)">Selecionar</button>
        </li>
      </ul>
    </div>
  `
})
export class UserSearchComponent {
  private api = inject(ApiService);
  @Output() userSelected = new EventEmitter<Usuario>();
  
  termo = '';
  usuarios: Usuario[] = [];

  pesquisar() {
    if (this.termo.length > 2) {
      this.api.buscarUsuarios(this.termo).subscribe(res => this.usuarios = res);
    }
  }

  selecionar(u: Usuario) {
    this.userSelected.emit(u);
    this.usuarios = [];
    this.termo = '';
  }
}