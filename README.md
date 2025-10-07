# Products – CRUD (.NET 8 + Angular 18)

CRUD de produtos com **.NET 8 Web API** (DDD-lite, EF Core InMemory, ProblemDetails, validação) e **Angular 18** (RxJS, formulários reativos, Materialize).

## Requisitos

- **.NET SDK 8.x**
- **Node 20+** e **pnpm** ou **npm**
- **Angular CLI 18**
  ```bash
  npm i -g @angular/cli@18
  ```

---

## 1) Backend (.NET 8 Web API)

### Estrutura (resumo)
```
src/
  Products.Api/
  Products.Application/
  Products.Domain/
  Products.Infrastructure/
tests/
  Products.Tests/
```

### Instalar dependências
Na raiz da solução:
```bash
dotnet restore
```

### Rodar a API (dev)
```bash
dotnet run --project src/Products.Api
```

- Swagger: `https://localhost:7264/swagger`
- Health: `GET https://localhost:7264/api/health`
- Base URL da API (para o front): `https://localhost:7264`

> **Banco:** EF Core **InMemory** (não precisa de migrations).

### Variáveis úteis (opcional)
- CORS está liberado para `http://localhost:4200`.
- ProblemDetails habilitado para respostas de erro padronizadas.

---

## 2) Testes (xUnit)

### Rodar testes
```bash
dotnet test
```

### Pacotes usados
- **xUnit**
- **FluentAssertions**
- **FluentValidation.TestHelper**
- **NSubstitute**

Cobertura foca em:
- **Domain**: invariantes (`Product`).
- **Validators**: regras dos *commands*.
- **Application**: cenários de sucesso/erro com repositório *mock*.

---

## 3) Frontend (Angular 18 + Materialize)

### Entrar na pasta do front
```bash
cd products-app
```

> Se seu projeto estiver com outro nome, ajuste o path acima.

### Instalar dependências
```bash
npm i
# ou pnpm i
```

### Materialize (já configurado)
Se precisar reinstalar:
```bash
npm i materialize-css
npm i -D @types/materialize-css sass
```
- `angular.json` inclui `materialize.min.js` em `scripts`.
- `styles.scss` importa o Materialize por SASS e aplica o tema.

### Configurar a URL da API
`src/environments/environment.ts`:
```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7264'
};
```

> O **interceptor** `api-url.interceptor.ts` prefixa `apiUrl` para chamadas que começam com `/`.

### Rodar o front (dev)
```bash
ng serve -o
```
- App: `http://localhost:4200`
- Rotas:
  - `/products` (lista)
  - `/products/new` (criar)
  - `/products/:id` (editar)

> Certifique-se de que a **API** está rodando em `https://localhost:7264`.

---

## 4) Scripts de atalho (opcional)

Na **raiz** do repositório, crie um `run-all.sh` (Linux/Mac):
```bash
#!/usr/bin/env bash
dotnet run --project src/Products.Api &
cd products-app && ng serve -o
```

No **Windows (PowerShell)**:
```powershell
Start-Process powershell -ArgumentList "dotnet run --project src/Products.Api"
Start-Process powershell -ArgumentList "cd products-app; ng serve -o"
```

Ou use **concurrently** via npm (na raiz do front):
```bash
npm i -D concurrently
```
`package.json` (do front):
```json
{
  "scripts": {
    "start": "ng serve -o",
    "start:all": "concurrently \"dotnet run --project ../src/Products.Api\" \"ng serve -o\""
  }
}
```

---

## 5) Convenções e decisões

- **RESTful**: `GET/POST/PUT/DELETE`, `201 Created` (POST), `204 No Content` (PUT/DELETE), ProblemDetails RFC-7807.
- **DDD-lite**: `Domain` limpo, `Application` com *commands/DTOs/services*, `Infrastructure` com EF, `Api` fina.
- **Validações**:
  - **FluentValidation** nos *commands* (entrada).
  - **Invariantes** no domínio (`Product` não aceita nome vazio, preço/estoque negativos).
- **UI**: Materialize com tema custom (header/footer `rgba(0,34,51,.95)`, tabela estilizada).
- **Angular**: RxJS, Reactive Forms, interceptors e rotas standalone.

---

## 6) Troubleshooting rápido

- **CORS**: se o front não consegue chamar a API, confira o `UseCors()` no `Program.cs` e a origem `http://localhost:4200`.
- **M não definido (Materialize)**:
  Garanta `materialize.min.js` em `angular.json` e o `main.ts` chamando `window.M?.AutoInit?.()`.
- **`*ngFor`/`formGroup` erros**:
  Importe `CommonModule`, `RouterModule` e `ReactiveFormsModule` nos **standalone components**.

---

## 7) Licença
Uso interno/educacional. Ajuste conforme sua necessidade.
