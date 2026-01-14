export interface Usuario {
  id: number;
  nome: string;
  email: string;
  senha?: string;
}

export interface AgendaGrupo {
  id?: number;
  grupoId?: number;
  refeicao: number; // Enum: 0=Cafe, 1=Almoco, 2=Jantar
  diaSemana?: number; // 0=Sunday...
  dataEspecifica?: string;
  ehRecorrente: boolean;
}

export interface Grupo {
  id: number;
  nome: string;
  capacidadeMaxima: number;
  criadorId: number;
  usuarios: Usuario[];
  agendas: AgendaGrupo[];
}

export interface ResumoRefeicaoDTO {
  data: string;
  tipo: string;
  descricao: string;
  quantidadePessoas: number;
  limite: number;
}