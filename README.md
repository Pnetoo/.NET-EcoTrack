# EcoTrack.Net — Sprint .NET (FIAP)
API de produtos do EcoTrack com arquitetura em camadas (Domain / Application / Infrastructure / WebApi). 
Pronta para rodar com Oracle via EF Core.

## 🎯 Objetivo
Expor um CRUD de **Products** com paginação e filtros, documentado em **Swagger**, usando **EF Core + Oracle**.

---

## 🧰 Pré-requisitos
- .NET SDK 8.x
- Visual Studio 2022 (ou VS Code)
- Oracle acessível (host/porta/SID ou service name) e um usuário com permissão para criar/usar a tabela `PRODUCTS` *(se você optar por criar via migrations)*.

---

## ⚙️ Configuração
Edite `EcoTrack.Net/appsettings.json` com sua connection string **Oracle**. Se usar **SID (ord)**, use o formato longo:
```json
{
  "ConnectionStrings": {
    "Oracle": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521)))(CONNECT_DATA=(SID=ord)))"
  }
}
```

> Alternativa com **Service Name**: `Data Source=oracle.fiap.com.br:1521/ord`

---

## 🗄️ Modelo de dados (se o banco NÃO for criado por migrations)
Crie manualmente no Oracle (DDL compatível com o código atual):

```sql
CREATE TABLE PRODUCTS (
  ID            RAW(16)        NOT NULL,
  NAME          NVARCHAR2(200) NOT NULL,
  CATEGORY      NVARCHAR2(120) NOT NULL,
  KCAL_100G     NUMBER(10,2),
  CO2_PER_UNIT  NUMBER(10,3),
  BARCODE       NVARCHAR2(64),
  CONSTRAINT PK_PRODUCTS PRIMARY KEY (ID)
);

CREATE INDEX IX_PRODUCTS_BARCODE ON PRODUCTS (BARCODE);
```

> O ID é `Guid` gerado pela aplicação (.NET). Não use trigger/sequence.

---

## 🏃 Como rodar
```bash
dotnet restore
dotnet build
# (opcional) criar via migrations
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project .\EcoTrack.Infrastructure --startup-project .\EcoTrack.Net
dotnet ef database update               --project .\EcoTrack.Infrastructure --startup-project .\EcoTrack.Net

# rodar a API com o perfil que abre o Swagger
dotnet run --project .\EcoTrack.Net --launch-profile "EcoTrack.Net"
```
Swagger: `https://localhost:5268/swagger` (ou a porta configurada no seu launchSettings).

---

## 🔗 Endpoints (Products)
Base: `/api/Products`

### POST `/api/Products`
Cria um produto.
```json
{
  "name": "Achocolatado Eco",
  "category": "Bebidas",
  "caloriesPer100g": 89.5,
  "co2PerUnit": 0.42,
  "barcode": "7891000123456"
}
```

### GET `/api/Products`
Consulta com paginação/filtros.
Parâmetros (query): `q`, `category`, `page` (default 1), `pageSize` (default 10).

### GET `/api/Products/{id}`
Retorna o produto por Id.

### PUT `/api/Products/{id}`
Atualiza dados do produto.
```json
{
  "name": "Achocolatado Eco Zero",
  "category": "Bebidas",
  "caloriesPer100g": 55.3,
  "co2PerUnit": 0.40,
  "barcode": "7891000123456"
}
```

### DELETE `/api/Products/{id}`
Remove um produto.

---

## 🧪 Coleção Postman
Importe a coleção `docs/EcoTrack.postman_collection.json` e ajuste a variável `{baseUrl}` conforme a porta do seu ambiente (ex.: `https://localhost:5268`).

---

## 🧱 Arquitetura
- **EcoTrack.Domain**: entidades e contratos (ex.: `Product`, `IProductRepository`).
- **EcoTrack.Application**: DTOs e serviços de aplicação (use-cases).
- **EcoTrack.Infrastructure**: EF Core, `DbContext`, repositórios Oracle, migrations.
- **EcoTrack.Net**: ASP.NET Core Web API + Swagger.

---

## 🧰 Troubleshooting
- 500 no POST → verifique se a tabela `PRODUCTS` existe e a connection string está correta.
- HTTPS redirect falhando → ajuste `launchSettings.json` e rode `dotnet dev-certs https --trust` ou comente `app.UseHttpsRedirection()` temporariamente.
- ORA-01017/12514 → usuário/senha/host/porta/SID/servicename incorretos.

---

## 📚 Referências rápidas
- EF Core Oracle Provider (Oracle.EntityFrameworkCore)
- Swashbuckle.AspNetCore (Swagger)

---

*Gerado em 2025-10-07.*
