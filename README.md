FoodGroups 🍲
Sistema de gerenciamento de grupos de alimentação, permitindo o controle de usuários, agendas de refeições e limites de capacidade.

🚀 Tecnologias Utilizadas
<ul>
    <li>
        .NET 10 (ASP.NET Core)
    </li>
    <li>
        Blazor Web App (Render mode: Interactive Server)
    </li>
    <li>
        Entity Framework Core com PostgreSQL (Npgsql)
    </li>
    <li>
        Swagger/OpenAPI para documentação da API
    </li>
    <li>
        DotNetEnv para gerenciamento de variáveis de ambiente
    </li>
</ul>

🛠️ Configuração do Ambiente
<ol>
    <li>
        Banco de Dados: Certifique-se de ter um banco PostgreSQL rodando.
    </li>
    <li>
        Variáveis de Ambiente: Crie um arquivo .env na raiz do projeto ou configure sua Connection String no appsettings.json. O sistema busca pela chave DB_CONNECTION_STRING.
    </li>
    <li>
        Migrations: Execute o comando para criar as tabelas: dotnet ef database update
    </li>
    <li>
        Execução: dotnet run
    </li>
</ol>

📍 Endpoints Principais (API)
<ul>
    <li>
        POST /api/Grupo: Cria um novo grupo.
    </li>
    <li>
        GET /api/Grupo/resumo-mensal: Retorna a agenda de refeições filtrada por mês/ano.
    </li>
    <li>
        POST /api/Grupo/{id}/adicionar-usuario: Adiciona um usuário a um grupo existente.
    </li>
</ul>