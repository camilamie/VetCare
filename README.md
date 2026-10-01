# VetCare API

API RESTful para gestão de tutores e pets de clínicas veterinárias, desenvolvida em **C# .NET 10** com **Entity Framework Core**.

> CP5 – C# Software Development

---

## 👥 Integrantes

Camila Mie Takara - RM555418
Guilherme Barbiero - RM555185
Marco Antonio Gonçalves - RM556818
Matheus Cantiere - RM558479
Vinicius Castro - RM556137

---

## 📌 Contexto do projeto

**O que é:** a VetCare API é o back-end de um sistema de cadastro para clínicas veterinárias. Ela permite registrar os **tutores** (donos dos animais) e os **pets** vinculados a cada um deles.

**Qual problema resolve:** muitas clínicas veterinárias de pequeno porte ainda controlam seus clientes em planilhas ou fichas de papel. Isso gera dados duplicados, dificuldade para localizar o histórico de um animal e informações de contato desatualizadas. A API centraliza esses dados, garante integridade (ex.: e-mail único por tutor, todo pet precisa ter um tutor válido) e disponibiliza tudo por endpoints REST que podem ser consumidos por qualquer front-end (web ou mobile).

**Para quem é destinado:** recepcionistas e veterinários de clínicas e pet shops, além de desenvolvedores que queiram integrar um front-end ou aplicativo ao cadastro da clínica.

---

## 🛠️ Tecnologias

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10
- **Banco de dados: SQLite** (arquivo local `vetcare.db`, sem necessidade de instalar servidor)
- Asp.Versioning (versionamento por URL: `/api/v1/...`)
- Swagger / Swashbuckle (documentação interativa)

---

## 🗂️ Estrutura do projeto

```
src/VetCare.Api
├── Controllers/V1/        # Endpoints da versão 1 da API
├── Data/
│   ├── AppDbContext.cs     # DbContext do EF Core
│   └── Configurations/     # Mapeamento das entidades (Fluent API) + seed
├── DTOs/                   # Objetos de entrada/saída com validações
├── Exceptions/             # Exceções de domínio (404, 400, 409)
├── Mappings/               # Conversão Entidade <-> DTO
├── Middleware/             # Tratamento global de erros (ProblemDetails)
├── Migrations/             # Migrations geradas pelo EF Core
├── Models/                 # Entidades: Tutor, Pet, Especie
├── Services/               # Regras de negócio e acesso a dados via EF Core
├── Program.cs              # Configuração da aplicação
└── VetCare.Api.http        # Requisições prontas para teste
```

**Fluxo de uma requisição:** `Controller → Service → AppDbContext (EF Core) → SQLite`.
Os controllers apenas recebem a requisição e devolvem o status HTTP; as regras ficam nos services; erros são lançados como exceções de domínio e convertidos em respostas padronizadas pelo `GlobalExceptionHandler`.

---

## ▶️ Como rodar localmente

### Pré-requisitos
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Ferramenta do EF Core:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Passo a passo

```bash
# 1. Clonar o repositório
git clone https://github.com/SEU-USUARIO/vetcare-api.git
cd vetcare-api/src/VetCare.Api

# 2. Restaurar dependências
dotnet restore

# 3. (Opcional) Criar/atualizar o banco manualmente
dotnet ef database update

# 4. Executar
dotnet run --launch-profile http
```

A aplicação aplica as migrations automaticamente ao iniciar, então o passo 3 é opcional.

Acesse o Swagger em: **http://localhost:5080/swagger**

Também é possível testar pelo arquivo `VetCare.Api.http` (VS Code com extensão REST Client, Visual Studio ou Rider).

---

## 🗄️ Migrations (EF Core)

| Migration | Descrição |
|-----------|-----------|
| `InitialCreate` | Cria as tabelas `Tutores` e `Pets`, a chave estrangeira `Pets.TutorId → Tutores.Id` com exclusão em cascata, o índice único em `Tutores.Email` e insere os dados iniciais (2 tutores e 3 pets). |

Comandos utilizados:

```bash
# Gerar a migration
dotnet ef migrations add InitialCreate

# Aplicar no banco
dotnet ef database update
```

### Modelo de dados

```
Tutores                         Pets
------------------------        ------------------------------
Id            INTEGER PK        Id              INTEGER PK
Nome          TEXT(100)         Nome            TEXT(60)
Email         TEXT(150) UNIQUE  Especie         TEXT(20)
Telefone      TEXT(20)          Raca            TEXT(60) NULL
DataCadastro  TEXT              DataNascimento  TEXT
                                PesoKg          REAL
                                TutorId         INTEGER FK → Tutores.Id
```

Relacionamento: **1 Tutor possui N Pets**.

---

## 🔗 Endpoints

Base URL: `http://localhost:7080/api/v1`

### Tutores

| Método | Rota | Descrição | Respostas |
|--------|------|-----------|-----------|
| GET | `/api/v1/tutores` | Lista todos os tutores | 200 |
| GET | `/api/v1/tutores/{id}` | Busca um tutor pelo id | 200, 404 |
| GET | `/api/v1/tutores/{id}/pets` | Lista os pets de um tutor | 200, 404 |
| POST | `/api/v1/tutores` | Cadastra um tutor | 201, 400, 409 |
| PUT | `/api/v1/tutores/{id}` | Atualiza um tutor | 204, 400, 404, 409 |
| DELETE | `/api/v1/tutores/{id}` | Remove um tutor e seus pets | 204, 404 |

### Pets

| Método | Rota | Descrição | Respostas |
|--------|------|-----------|-----------|
| GET | `/api/v1/pets` | Lista os pets (filtro opcional `?especie=Gato`) | 200 |
| GET | `/api/v1/pets/{id}` | Busca um pet pelo id | 200, 404 |
| POST | `/api/v1/pets` | Cadastra um pet | 201, 400 |
| PUT | `/api/v1/pets/{id}` | Atualiza um pet | 204, 400, 404 |
| DELETE | `/api/v1/pets/{id}` | Remove um pet | 204, 404 |

Valores aceitos para `especie`: `Cachorro`, `Gato`, `Ave`, `Roedor`, `Reptil`, `Outro`.

### Exemplos de corpo

**POST /api/v1/tutores**
```json
{
  "nome": "Mariana Alves",
  "email": "mariana.alves@email.com",
  "telefone": "(11) 99876-5432"
}
```

**POST /api/v1/pets**
```json
{
  "nome": "Paçoca",
  "especie": "Cachorro",
  "raca": "Vira-lata",
  "dataNascimento": "2024-03-15",
  "pesoKg": 12.4,
  "tutorId": 1
}
```

### Status codes utilizados

| Código | Quando ocorre |
|--------|---------------|
| 200 OK | Consulta realizada com sucesso |
| 201 Created | Recurso criado (retorna o recurso e o header `Location`) |
| 204 No Content | Atualização ou remoção realizada com sucesso |
| 400 Bad Request | Dados inválidos ou regra de negócio violada (ex.: tutor inexistente, nascimento no futuro) |
| 404 Not Found | Recurso não encontrado |
| 409 Conflict | E-mail de tutor já cadastrado |
| 500 Internal Server Error | Erro inesperado (tratado e registrado em log) |

Todas as respostas de erro seguem o padrão **ProblemDetails** (RFC 9457):

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "Tutor com id 999 não encontrado.",
  "instance": "/api/v1/tutores/999"
}
```
