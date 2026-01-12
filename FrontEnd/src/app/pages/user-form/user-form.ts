import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-user-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css',
})
export class UserForm implements OnInit {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isEdit = false;
  // Objeto modelo para o formulário
  usuario: any = { nome: '', email: '', senha: '' };

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      // Ao editar, buscamos o usuário existente
      this.api.obterUsuario(+id).subscribe(u => {
        this.usuario = u;
        // Limpa a senha para não enviar hash de volta se não for alterada
        // (Nota: No seu backend atual, a edição de senha não está implementada no PUT, apenas Nome/Email)
        this.usuario.senha = ''; 
      });
    }
  }

  salvar() {
    if (this.isEdit) {
      this.api.atualizarUsuario(this.usuario.id, this.usuario).subscribe(() => {
        this.router.navigate(['/usuarios']);
      });
    } else {
      this.api.criarUsuario(this.usuario).subscribe(() => {
        this.router.navigate(['/usuarios']);
      });
    }
  }
}
