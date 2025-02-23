# TaskManagementAPI

## Descrição

Este projeto é uma aplicação de gerenciamento de tarefas, construída com ASP.NET Core, onde os usuários podem visualizar, adicionar, editar e filtrar tarefas. Ele permite o gerenciamento de tarefas com filtros dinâmicos, utilizando filtros como status e data de vencimento.

### Funcionalidades
- Filtrar tarefas por status e data de vencimento.
- Visualizar tarefas em formato de lista.
- Adicionar novas tarefas com informações detalhadas.
- Editar tarefas existentes.

### Tecnologias Utilizadas
- ASP.NET Core para a API backend.
- Entity Framework Core InMemory para interação com banco de dados em memória.
- Moq para testes unitários.
- xUnit para framework de testes.

### Instalação

1. Clone este repositório para a sua máquina local:
    ```bash
    git clone https://github.com/doploxoown/TesteSada.git

2. Navegue até o diretório do projeto:
    ```bash
    cd TesteSada

3. Restaure os pacotes NuGet:
    ```bash
    dotnet restore

4. Compile o projeto:
    ```bash
   dotnet build`

## Testes
O projeto inclui testes unitários para a API, que podem ser executados usando o xUnit.

Executando os testes:
  - `dotnet test`

## Uso

1. Inicie a aplicação:
    ```bash
    dotnet run --project TaskManagementAPI

2. Acesse a API em `http://localhost:5125/api/task`.

3. Para acessar o swagger acesse: `http://localhost:5125/swagger/index.html` 

### Endpoints

- **GET** `/api/task` - Obtém todas as tarefas.
- **GET** `/api/task/{id}` - Obtém uma tarefa pelo ID.
- **GET** `/api/task/filter?status={status}&dueDate={dueDate}` - Filtra as tarefas pelo status e/ou data de vencimento.
- **POST** `/api/task` - Cria uma nova tarefa.
- **PUT** `/api/task/{id}` - Atualiza uma tarefa existente.
- **DELETE** `/api/task/{id}` - Remove uma tarefa pelo ID.

### Exemplo de Requisição para Criar uma Tarefa

```json
POST /api/task
{
 "title": "Nova Tarefa",
 "description": "Descrição detalhada da nova tarefa",
 "dueDate": "2025-12-31T23:59:59Z",
 "status": 0
}
