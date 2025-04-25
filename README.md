# Polly Resilience API

Este projeto é uma API baseada em .NET 8 que utiliza o [Polly](https://github.com/App-vNext/Polly) para implementar padrões de resiliência como Retry, Circuit Breaker, Timeout e Bulkhead. O objetivo é demonstrar a aplicação de políticas de resiliência em requisições HTTP, garantindo que a API seja capaz de lidar com falhas temporárias e erros com maior robustez.

## Funcionalidades

- **Retry Policy**: Reintenta uma requisição HTTP falhada até um número máximo de tentativas com um intervalo de tempo entre as tentativas.
- **Circuit Breaker**: Quando o número de falhas consecutivas excede um limite, a API "abre o circuito" e impede novas tentativas por um tempo determinado.
- **Timeout**: Define um tempo limite para a resposta da requisição HTTP.
- **Bulkhead**: Limita o número de requisições paralelas que podem ser feitas, prevenindo que um pico de carga sobrecarregue o sistema.

## Estrutura do Projeto

O projeto é estruturado da seguinte forma:

- **ResilienceController**: Controller para simular as requisições.
- **PollyResilienceApp.Policies**: Contém as políticas de resiliência configuradas usando o Polly.
- **PollySettings**: Contém as configurações relacionadas às políticas de resiliência, como o número de tentativas, o tempo de espera, etc.
- **Program.cs**: Onde as configurações e as políticas são aplicadas ao `HttpClient`.

## Pré-requisitos

Antes de rodar o projeto, é necessário ter o seguinte instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Configuração

As configurações de resiliência são definidas no arquivo `appsettings.json`. O arquivo contém configurações como:

- **Retry**: Configura o número de tentativas e o tempo de espera entre as tentativas.
- **CircuitBreaker**: Define o limiar de falhas para abrir o circuito e o tempo de duração do "break".
- **Timeout**: Configura o tempo máximo de espera para uma requisição.
- **Bulkhead**: Define a quantidade máxima de requisições simultâneas e a quantidade máxima de requisições na fila.

### Exemplo de `appsettings.json`

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

## Como Executar

### Passo 1: Clone o repositório

Clone o repositório para o seu ambiente local:

```bash
git clone https://github.com/pedrinhoas7/PollyResilienceApp.git
cd PollyResilienceApp
```

### Passo 2: Instale as dependências

Use o seguinte comando para restaurar as dependências do projeto:

```bash
dotnet restore
```

### Passo 3: Execute o projeto

Execute o projeto com o comando:

```bash
dotnet run
```

A API estará disponível em `https://localhost:7048/swagger/index.html`.

## Endpoints

### `GET /Resillience`

Este endpoint faz uma requisição HTTP para um serviço externo, utilizando as políticas configuradas com Polly.

- **Políticas aplicadas**:
  - Retry: Reintenta a requisição até x vezes.
  - Circuit Breaker: Se o serviço falhar em 50% das requisições, o circuito será aberto por x segundos.
  - Timeout: Limita a resposta a x segundos.
  - Bulkhead: Limita o número de requisições simultâneas a x.

### Exemplo de resposta

#### Sucesso

```json
{
  "userId": 1,
  "id": 2,
  "title": "quis ut nam facilis et officia qui",
  "completed": false
}
```

👨‍💻 Autor
Desenvolvido com 💙 por Pedro Henrique
