import { Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard/dashboard';
import { GrupoForm } from './pages/grupo-form/grupo-form';
import { UserForm } from './pages/user-form/user-form';
import { UserList } from './pages/user-list/user-list';

export const routes: Routes = [
  { path: '', component: Dashboard },
  
  // Rotas de Grupos
  { path: 'grupos/criar', component: GrupoForm },
  { path: 'grupos/editar/:id', component: GrupoForm },

  // Rotas de Usuários
  { path: 'usuarios', component: UserList },
  { path: 'usuarios/criar', component: UserForm },
  { path: 'usuarios/editar/:id', component: UserForm },
];