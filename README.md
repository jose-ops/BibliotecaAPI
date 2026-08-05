# Sistema de Biblioteca - Backend

API REST desenvolvida em .NET 8 para gerenciamento de biblioteca com sistema de autenticação, controle de acesso por roles e upload de imagens para AWS S3.

---

Tecnologias Utilizadas

 Framework e Linguagem
- .NET 8 - Framework principal
- C#*- Linguagem de programação
- ASP.NET Core Web API - Para criação da API REST

 Banco de Dados
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados relacional
- **Code First Migrations** - Controle de versionamento do banco

 Autenticação e Segurança
- **ASP.NET Core Identity** - Sistema de autenticação
- **JWT (JSON Web Tokens)** - Autenticação stateless
- **BCrypt.NET** - Hash de senhas
- **Role-Based Authorization** - Controle de acesso (Admin/User)

 Cloud e Storage
- **AWS S3** - Armazenamento de imagens
- **AWSSDK.S3** - SDK oficial da AWS para .NET

 Outros
- **CORS** - Permitir requisições do frontend
- **Swagger/OpenAPI** - Documentação da API

---

 Estrutura do Projeto

```
Backend/
├── Controllers/
│   ├── AuthController.cs       # Login, registro
│   ├── LivrosController.cs     # CRUD de livros
│   └── AutoresController.cs    # Listagem de autores
├── Models/
│   ├── Livro.cs               # Entidade Livro
│   ├── Autor.cs               # Entidade Autor
│   └── DTOs/
│       ├── LoginDto.cs
│       └── RegisterDto.cs
├── Data/
│   └── ApplicationDbContext.cs # Contexto do EF Core
├── Services/
│   ├── S3Service.cs           # Upload para AWS S3
│   └── TokenService.cs        # Geração de JWT
├── appsettings.json           # Configurações
└── Program.cs                 # Configuração da aplicação
```

---

 Configuração e Instalação

 1. **Pré-requisitos**

- .NET 8 SDK instalado
- SQL Server (local ou remoto)
- Conta AWS com bucket S3 criado
- Visual Studio 2022 ou VS Code

 2. **Pacotes NuGet Necessários**

```bash
# Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# JWT
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# AWS S3
dotnet add package AWSSDK.S3

# BCrypt
dotnet add package BCrypt.Net-Next
```

3. **Configurar appsettings.json**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "SUA_CHAVE_SECRETA_SUPER_SEGURA_AQUI_COM_PELO_MENOS_32_CARACTERES",
    "ExpirationInMinutes": 60
  },
  "AWS": {
    "AccessKey": "SUA_AWS_ACCESS_KEY",
    "SecretKey": "SUA_AWS_SECRET_KEY",
    "BucketName": "seu-bucket-name",
    "Region": "us-east-1"
  }
}
```

4. **Executar Migrations**

```bash
# Criar migration inicial
dotnet ef migrations add InitialCreate

# Aplicar ao banco
dotnet ef database update
```

5. **Executar o Projeto**

```bash
dotnet run
```

API estará disponível em: `https://localhost:7086`

---

 Autenticação

### Registrar Usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@email.com",
  "nome": "Nome do Usuário",
  "password": "SenhaSegura@123"
}
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@email.com",
  "password": "SenhaSegura@123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "usuario@email.com",
  "nome": "Nome do Usuário",
  "role": "User",
  "expiresAt": "2026-02-13T15:30:00Z"
}
```

---

 Endpoints da API

 **Livros**

 Listar todos os livros
```http
GET /api/livros
Authorization: Bearer {token}
```

 Buscar livro por ID
```http
GET /api/livros/{id}
Authorization: Bearer {token}
```

 Criar novo livro (Admin)
```http
POST /api/livros
Authorization: Bearer {token}
Content-Type: application/json

{
  "titulo": "Nome do Livro",
  "autorId": 1,
  "disponivel": true
}
```

 Atualizar livro (Admin)
```http
PUT /api/livros/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "titulo": "Nome Atualizado",
  "autorId": 1,
  "disponivel": false
}
```

 Deletar livro (Admin)
```http
DELETE /api/livros/{id}
Authorization: Bearer {token}
```

 Upload de imagem (Admin)
```http
PUT /api/livros/{id}/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

file: [arquivo de imagem]
```

 **Autores**

 Listar autores
```http
GET /api/autores
Authorization: Bearer {token}
```

---

  Roles e Permissões

| Endpoint | User | Admin |
|----------|------|-------|
| GET /api/livros | ✅ | ✅ |
| GET /api/livros/{id} | ✅ | ✅ |
| POST /api/livros | ❌ | ✅ |
| PUT /api/livros/{id} | ❌ | ✅ |
| DELETE /api/livros/{id} | ❌ | ✅ |
| PUT /api/livros/{id}/upload | ❌ | ✅ |

---

Configuração AWS S3

1. Criar Bucket S3
- Nome: `biblioteca-imagens` (ou outro nome único)
- Região: `us-east-1` (ou sua preferência)

 2. Configurar Permissões IAM

Criar usuário IAM com a seguinte política:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "s3:PutObject",
        "s3:GetObject",
        "s3:DeleteObject"
      ],
      "Resource": "arn:aws:s3:::seu-bucket-name/*"
    }
  ]
}
```

3. Configurar CORS no Bucket

```json
[
  {
    "AllowedHeaders": ["*"],
    "AllowedMethods": ["GET", "PUT", "POST", "DELETE"],
    "AllowedOrigins": ["*"],
    "ExposeHeaders": []
  }
]
```

---

Modelo de Dados

Livro
```csharp
public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public bool Disponivel { get; set; }
    public string? ImageUrl { get; set; }
    public string? Descricao { get; set; }
    
    public int AutorId { get; set; }
    public Autor Autor { get; set; }
}
```

Autor
```csharp
public class Autor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Nacionalidade { get; set; }
    
    public ICollection<Livro> Livros { get; set; }
}
```

---

Testando a API

Com Swagger
Acesse: `https://localhost:7086/swagger`

Com Postman
1. Faça login em `/api/auth/login`
2. Copie o token retornado
3. Use nas requisições: `Authorization: Bearer {token}`

---

Troubleshooting

Erro de CORS
Certifique-se de que `app.UseCors("AllowAll")` está **antes** de `app.UseAuthorization()` no `Program.cs`

Erro de conexão com S3
- Verifique as credenciais AWS no `appsettings.json`
- Confirme que o bucket existe e está na região correta
- Verifique as permissões do usuário IAM

Erro 401 Unauthorized
- Token expirado (faça login novamente)
- Token inválido (verifique se está no formato `Bearer {token}`)

---

Este projeto foi desenvolvido para fins educacionais.
