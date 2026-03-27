# InventoryAnalyzerAPI

Uma API RESTful desenvolvida com **ASP.NET Core** para análise de movimentações de estoque a partir de arquivos CSV. A API processa entradas e saídas de produtos, calcula o estoque atual por produto e detecta anomalias como ocorrências de estoque negativo.

---

## 📋 Índice

- [Funcionalidades](#funcionalidades)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Referência da API](#referência-da-api)
- [Formato do CSV](#formato-do-csv)
- [Formato da Resposta](#formato-da-resposta)
- [Tratamento de Erros](#tratamento-de-erros)
- [Configuração](#configuração)

---

## ✨ Funcionalidades

- Upload de arquivo CSV com registros de movimentação de estoque
- Cálculo da quantidade final de estoque por produto
- Detecção e reporte de anomalias (ex: estoque ficando negativo)
- Informa o menor nível de estoque atingido quando uma anomalia ocorre
- Validação de tipo e tamanho do arquivo antes do processamento
- Swagger UI disponível no ambiente de desenvolvimento

---

## 🛠 Tecnologias

| Tecnologia | Versão |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10.0 |
| CsvHelper | 33.0.1 |
| Swashbuckle (Swagger) | 6.6.2 |

---

## 📁 Estrutura do Projeto

```
InventoryAnalyzerAPI/
├── Controllers/
│   └── InventoryController.cs       # Gerencia as requisições HTTP
├── DTOs/
│   ├── AnomalyDto.cs                # Modelo de resposta de anomalia
│   ├── InventoryAnalysisResultDto.cs # Modelo completo do resultado da análise
│   ├── InventoryRecordDto.cs        # Modelo de linha do CSV parseada
│   └── StockItemDto.cs              # Modelo de item de estoque
├── Enums/
│   └── MovementType.cs              # Tipos de movimentação: In / Out
├── Services/
│   ├── Interfaces/
│   │   ├── ICsvParseService.cs      # Contrato do serviço de parse de CSV
│   │   └── IInventoryService.cs    # Contrato do serviço de análise
│   ├── CsvParseService.cs          # Implementação do parse de CSV
│   └── InventoryService.cs         # Lógica de análise de estoque
├── Program.cs                       # Bootstrap da aplicação e configuração de DI
└── appsettings.json
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Executando localmente

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/InventoryAnalyzerAPI.git
cd InventoryAnalyzerAPI

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run --project InventoryAnalyzerAPI
```

A API estará disponível em `https://localhost:7000` (ou na porta exibida no terminal).

O Swagger UI estará acessível em `https://localhost:7000/swagger` ao executar no modo Development.

---

## 📡 Referência da API

### POST `/api/inventory/analyze`

Faz o upload de um arquivo CSV e retorna a análise completa do estoque.

**Requisição**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `file` | `multipart/form-data` | Arquivo CSV com os registros de movimentação |

**Restrições**
- O arquivo deve ter extensão `.csv`
- O `Content-Type` deve ser `text/csv`, `application/csv` ou `text/plain`
- Tamanho máximo do arquivo: **10 MB**

**Exemplo (curl)**

```bash
curl -X POST https://localhost:7000/api/inventory/analyze \
  -F "file=@movimentacoes.csv;type=text/csv"
```

---

## 📄 Formato do CSV

O arquivo CSV deve ter uma linha de cabeçalho seguida pelas linhas de dados com exatamente 5 colunas:

```
Timestamp,ProductId,ProductName,Type,Quantity
1710000000,P001,Widget A,In,100
1710001000,P002,Widget B,In,50
1710002000,P001,Widget A,Out,30
1710003000,P002,Widget B,Out,80
```

| Coluna | Tipo | Descrição |
|---|---|---|
| `Timestamp` | `long` | Timestamp Unix da movimentação |
| `ProductId` | `string` | Identificador único do produto |
| `ProductName` | `string` | Nome legível do produto |
| `Type` | `string` | Direção da movimentação: `In` ou `Out` (sem distinção de maiúsculas) |
| `Quantity` | `int` | Quantidade de unidades movimentadas |

> **Observação:** Campos que contenham vírgulas devem estar entre aspas duplas (tratado automaticamente pelo CsvHelper).

---

## 📦 Formato da Resposta

**Sucesso — `200 OK`**

```json
{
  "stock": [
    {
      "productId": "P001",
      "productName": "Widget A",
      "quantity": 70
    },
    {
      "productId": "P002",
      "productName": "Widget B",
      "quantity": -30
    }
  ],
  "anomalies": [
    {
      "productId": "P002",
      "productName": "Widget B",
      "message": "Stock went negative (reached -30)"
    }
  ]
}
```

| Campo | Descrição |
|---|---|
| `stock` | Quantidade final de estoque de cada produto encontrado no arquivo |
| `anomalies` | Produtos cujo estoque ficou negativo em algum momento, com o menor valor atingido |

---

## ⚠️ Tratamento de Erros

| Status | Condição |
|---|---|
| `400 Bad Request` | Nenhum arquivo enviado, arquivo vazio, extensão incorreta ou tipo de conteúdo inválido |

**Exemplo de resposta de erro**

```json
"Only CSV files are allowed."
```

---

## ⚙️ Configuração

### Limite de tamanho de upload

O tamanho máximo de upload permitido é configurado no `Program.cs` via `FormOptions`:

```csharp
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});
```

Para aumentar ou diminuir o limite, altere esse valor antes de fazer o deploy.
