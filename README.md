# InventoryAnalyzerAPI

Uma API RESTful desenvolvida com **ASP.NET Core** para análise de movimentações de estoque a partir de arquivos CSV, com frontend em **React** para visualização dos resultados. A API processa entradas e saídas de produtos, calcula o estoque atual por produto e detecta anomalias como ocorrências de estoque negativo.

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
- Validação de tipo e tamanho do arquivo no lado do cliente, antes do envio
- Validação de tipo e tamanho do arquivo no lado do servidor, antes do processamento
- Respostas de erro padronizadas no formato `ProblemDetails` (RFC 7807)
- Swagger UI disponível no ambiente de desenvolvimento
- Interface web com dark mode para visualização dos resultados
- Suporte a drag and drop para upload do arquivo CSV

---

## 🛠 Tecnologias

### Backend

| Tecnologia | Versão |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10.0 |
| CsvHelper | 33.0.1 |
| Swashbuckle (Swagger) | 6.6.2 |

### Frontend

| Tecnologia | Versão |
|---|---|
| React | 18.3.1 |
| TypeScript | 5.2.2 |
| Vite | 5.3.1 |
| Axios | 1.7.2 |

---

## 📁 Estrutura do Projeto

```
InventoryAnalyzerAPI/
├── InventoryAnalyzerAPI/                        # Projeto backend
│   ├── Controllers/
│   │   └── InventoryController.cs               # Gerencia as requisições HTTP
│   ├── DTOs/
│   │   ├── AnomalyDto.cs                        # Modelo de resposta de anomalia
│   │   ├── InventoryAnalysisResultDto.cs         # Modelo completo do resultado da análise
│   │   ├── InventoryRecordDto.cs                 # Modelo de linha do CSV parseada
│   │   └── StockItemDto.cs                       # Modelo de item de estoque
│   ├── Enums/
│   │   └── MovementType.cs                       # Tipos de movimentação: In / Out
│   ├── Services/
│   │   ├── Interfaces/
│   │   │   ├── ICsvParseService.cs               # Contrato do serviço de parse de CSV
│   │   │   └── IInventoryService.cs              # Contrato do serviço de análise
│   │   ├── CsvParseService.cs                    # Implementação do parse de CSV
│   │   └── InventoryService.cs                   # Lógica de análise de estoque
│   ├── Program.cs                                # Bootstrap da aplicação e configuração de DI
│   ├── appsettings.json                          # Configurações gerais (CORS, upload limit)
│   └── appsettings.Development.json              # Overrides de logging para desenvolvimento
│
└── InventoryAnalyzerWeb/                         # Projeto frontend
    ├── src/
    │   ├── components/
    │   │   ├── UploadZone.tsx                    # Área de drag and drop do CSV
    │   │   ├── StockTable.tsx                    # Tabela com estoque atual
    │   │   └── AnomaliesList.tsx                 # Lista de anomalias detectadas
    │   ├── pages/
    │   │   └── HomePage.tsx                      # Página principal com toda a lógica
    │   ├── services/
    │   │   └── inventoryService.ts               # Chamada para a API com Axios
    │   ├── types/
    │   │   └── index.ts                          # Tipos TypeScript espelhando os DTOs do backend
    │   ├── vite-env.d.ts                         # Declaração de tipos das variáveis de ambiente Vite
    │   ├── App.tsx
    │   ├── main.tsx
    │   └── index.css                             # Tema dark com CSS variables
    ├── .env.development                          # Variáveis de ambiente para desenvolvimento
    ├── .env.example                              # Documentação das variáveis de ambiente
    ├── index.html
    ├── package.json
    ├── vite.config.ts                            # Proxy configurado via variável de ambiente
    └── tsconfig.json
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org)

### Backend

```bash
# Clone o repositório
git clone https://github.com/LcsAlmeidaS/InventoryAnalyzerAPI.git
cd InventoryAnalyzerAPI/InventoryAnalyzerAPI

# Restaure as dependências e execute
dotnet restore
dotnet run
```

A API estará disponível na porta exibida no terminal, geralmente `http://localhost:5122`.

O Swagger UI estará acessível em `http://localhost:5122/swagger` ao executar no modo Development.

### Frontend

Em um novo terminal:

```bash
cd InventoryAnalyzerWeb

# Instale as dependências
npm install

# Execute o servidor de desenvolvimento
npm run dev
```

O frontend estará disponível em `http://localhost:5173` e se comunica automaticamente com o backend via proxy configurado no `vite.config.ts`.

> **Atenção:** certifique-se de que o backend está rodando antes de usar o frontend.

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
curl -X POST http://localhost:5122/api/inventory/analyze \
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
| `ProductId` | `string` | Identificador único do produto — não pode ser vazio |
| `ProductName` | `string` | Nome legível do produto |
| `Type` | `string` | Direção da movimentação: `In` ou `Out` (sem distinção de maiúsculas) |
| `Quantity` | `int` | Quantidade de unidades movimentadas — deve ser maior que zero |

> **Observação:** linhas com `ProductId` vazio ou `Quantity` menor ou igual a zero são descartadas silenciosamente. Campos que contenham vírgulas devem estar entre aspas duplas (tratado automaticamente pelo CsvHelper).

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

Todos os erros seguem o formato `ProblemDetails` (RFC 7807):

| Status | Condição |
|---|---|
| `400 Bad Request` | Nenhum arquivo enviado, arquivo vazio, extensão incorreta ou tipo de conteúdo inválido |
| `500 Internal Server Error` | Erro inesperado no servidor |

**Exemplo de resposta de erro**

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Invalid file type.",
  "detail": "Only CSV files are allowed. Accepted content types: text/csv, application/csv, text/plain.",
  "status": 400
}
```

---

## ⚙️ Configuração

### Backend

As principais configurações ficam no `appsettings.json`:

```json
{
  "Cors": {
    "AllowedOrigins": [ "http://localhost:5173" ]
  },
  "Upload": {
    "MaxFileSizeBytes": 10485760
  }
}
```

Para alterar as origens permitidas no CORS ou o limite de tamanho de upload, edite esses campos — não é necessário recompilar.

### Frontend

As variáveis de ambiente do frontend ficam no arquivo `.env.development` (desenvolvimento) ou em variáveis definidas na plataforma de deploy (produção). Copie `.env.example` como ponto de partida:

```bash
cp .env.example .env.development
```

| Variável | Descrição |
|---|---|
| `VITE_API_URL` | URL base da API consumida pelo frontend. Em dev use `/api` (roteado pelo proxy do Vite). Em produção, use a URL pública completa. |
| `BACKEND_URL` | URL do backend usada pelo proxy do Vite em desenvolvimento. Altere se a porta do backend mudar. |

**Exemplo de `.env.development`**

```
VITE_API_URL=/api
BACKEND_URL=http://localhost:5122
```
