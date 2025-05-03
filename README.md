# Projeto da API

Este projeto é uma API construída utilizando C# e ASP.NET Core, e serve como um backend para gerenciamento de dados relacionados aos _Run Points_ e _Location Points_. Ele inclui autenticação via JWT e funcionalidades para criação e leitura de dados.

## Requisitos

Para rodar este projeto, você precisará de:

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) (opcional, se for usar containers)
- Banco de dados configurado no `appsettings.json`

## Instalação

1. Clone o repositório para sua máquina local:

   ```bash
   git clone https://github.com/seuusuario/seu-repositorio.git
   cd seu-repositorio
   ```

2. Restaure as dependências do projeto:

   ```bash
   dotnet restore
   ```

3. Se estiver utilizando Docker, execute o comando abaixo para subir o banco de dados (se necessário):

   ```bash
   docker-compose up -d
   ```

4. Se você ainda não tem o banco de dados configurado, crie as migrações e aplique-as:

   ```bash
   dotnet ef migrations add InicialCreate
   dotnet ef database update
   ```

## Configuração de Ambiente

Crie o arquivo `appsettings.json` e configure os dados necessários, como a string de conexão ao banco de dados e qualquer outra variável de ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SeuBancoDeDadosConnectionString"
  },
  "JwtSettings": {
    "Secret": "SuaChaveSecreta"
  }
}
```

Não se esqueça de adicionar esse arquivo ao seu `.gitignore` para evitar o envio de dados sensíveis para o repositório.

## Como Rodar

Após configurar o banco de dados e o ambiente, execute o projeto com o comando:

```bash
dotnet run
```

Isso iniciará a API no endereço `http://localhost:5000` (ou outro, dependendo da configuração do seu projeto).

## Endpoints

Aqui estão alguns exemplos de endpoints disponíveis:

### 1. **Login**

**POST /api/auth/login**

Recebe um email e senha, e retorna um token JWT para autenticação.

**Exemplo de Request**:

```json
{
  "email": "usuario@exemplo.com",
  "password": "senha-secreta"
}
```

**Resposta**:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 2. **Criar Local Point**

**POST /api/locationpoints**

Cria um novo local no sistema.

**Exemplo de Request**:

```json
{
  "name": "Local 1",
  "latitude": 40.7128,
  "longitude": -74.006
}
```

**Resposta**:

```json
{
  "id": 1,
  "name": "Local 1",
  "latitude": 40.7128,
  "longitude": -74.006
}
```

### 3. **Listar Runs**

**GET /api/runs**

Retorna a lista de todos os "runs" registrados.

**Resposta**:

```json
[
  {
    "id": 1,
    "name": "Run 1",
    "locationId": 1,
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

## Contribuindo

Se você deseja contribuir para este projeto, fique à vontade para abrir um **pull request** ou relatar problemas através das **issues**.

1. Faça um fork do repositório
2. Crie uma branch para a sua modificação (`git checkout -b minha-modificacao`)
3. Comite suas mudanças (`git commit -am 'Adiciona nova feature'`)
4. Envie para a branch do repositório remoto (`git push origin minha-modificacao`)
5. Abra um pull request!

## Licença

Distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais informações.
