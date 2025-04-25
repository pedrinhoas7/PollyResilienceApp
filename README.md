# 🦜Polly Resilience API

Este projeto é uma API em **.NET 8** que utiliza o [Polly](https://github.com/App-vNext/Polly) para implementar padrões de resiliência como **Retry**, **Circuit Breaker**, **Timeout** e **Bulkhead**.  
O objetivo é demonstrar como proteger aplicações de falhas temporárias e garantir maior robustez em chamadas HTTP.

---

## ✨ Funcionalidades

- **Retry Policy**: Tenta novamente uma requisição falha um número configurável de vezes.
- **Circuit Breaker**: Abre o circuito após um número limite de falhas consecutivas, bloqueando novas requisições temporariamente.
- **Timeout Policy**: Garante que requisições não fiquem presas indefinidamente.
- **Bulkhead Policy**: Controla o número máximo de requisições simultâneas, isolando sobrecargas.

---

## 📂 Estrutura do Projeto

- **ResilienceController**: Controller que expõe endpoints para simular cenários de falha e resiliência.
- **PollyConfigPolicyBuilder**: Responsável por construir e aplicar as políticas do Polly.
- **Program.cs**: Configuração da API, HttpClient e injeções de dependência.

---

## 🔧 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## ⚙️ Configurações

As políticas de resiliência são configuradas via `appsettings.json`. Exemplo:

```json
{
  "Polly": {
    "CircuitBreaker": {
      "DurationOfBreak": "00:00:30",
      "FailureThreshold": 5,
      "MinimumThroughput": 10
    },
    "Retry": {
      "Count": 10,
      "Delay": "00:00:10"
    },
    "Timeout": {
      "Duration": "00:00:30"
    },
    "Bulkhead": {
      "MaxParallelization": 5,
      "MaxQueuingActions": 10
    }
  }
}
```

---

## 🚀 Como Executar

1. **Clone o repositório**:

```bash
git clone https://github.com/pedrinhoas7/PollyResilienceApp.git
cd PollyResilienceApp
```

2. **Restaure as dependências**:

```bash
dotnet restore
```

3. **Execute o projeto**:

```bash
dotnet run
```

A API estará disponível em:  
👉 `https://localhost:7048/swagger/index.html`

---

## 🛠️ Endpoints Disponíveis

### 🔄 `GET /Simulate/RetryAndCircuitBreak`

- **Descrição**: 
  - Simula falhas contínuas para ativar a política de **Retry** até o limite e, eventualmente, acionar o **Circuit Breaker**.
- **Resposta esperada**: 
  - Uma exceção indicando que o circuito foi aberto após repetidas falhas.

---

### 🔁 `GET /Simulate/Retry`

- **Descrição**: 
  - Simula múltiplas tentativas de requisição usando apenas a política de **Retry**.
- **Exemplo de resposta**:

```json
[
  "Retry 1 after 00:00:05",
  "Retry 2 after 00:00:05",
  "Retry 3 after 00:00:05",
  "Retry 4 after 00:00:05",
  "Retry 5 after 00:00:05",
  "Retry 6 after 00:00:05",
  "Response: InternalServerError"
]
```

---

### 🧱 `GET /Simulate/BulkHead`

- **Descrição**: 
  - Simula várias requisições simultâneas para demonstrar o funcionamento do **Bulkhead**, aceitando algumas e rejeitando outras.
- **Exemplo de resposta**:

```json
[
  "Task 0: OK",
  "Task 1: OK",
  "Task 2: OK",
  "Task 3: OK",
  "Task 4: OK",
  "Task 5: OK",
  "Task 6: Rejeitada pelo Bulkhead",
  "Task 7: Rejeitada pelo Bulkhead",
  "Task 8: Rejeitada pelo Bulkhead",
  "Task 9: Rejeitada pelo Bulkhead",
  "Task 10: Rejeitada pelo Bulkhead",
  "Task 11: Rejeitada pelo Bulkhead",
  "Task 12: Rejeitada pelo Bulkhead",
  "Task 13: Rejeitada pelo Bulkhead",
  "Task 14: Rejeitada pelo Bulkhead"
]
```

---

## ⚡ Políticas aplicadas

- **Retry**: Reenvia a requisição falha até o número configurado de vezes.
- **Circuit Breaker**: Abre o circuito após atingir o limite de falhas (Ex: 50% das chamadas).
- **Timeout**: Aborta chamadas que ultrapassam o tempo limite configurado.
- **Bulkhead**: Limita a quantidade de requisições simultâneas, isolando sobrecargas.


## 👨‍💻 Autor

Desenvolvido com 💙 por **Pedro Henrique**  
[LinkedIn](https://www.linkedin.com/in/seu-perfil/) | [GitHub](https://github.com/pedrinhoas7)
