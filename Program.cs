using PollyResilienceApp.Configurations;
using PollyResilienceApp.Interfaces;
using PollyResilienceApp.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IResillienceService, ResillienceService>();


// Adicionando a configuração PollySettings no container
builder.Services.Configure<PollySettings>(builder.Configuration.GetSection("Polly"));

// Configurando o HttpClient para usar as políticas do Polly
builder.Services.AddHttpClient("Client", client =>
{
    client.BaseAddress = new Uri("https://httpstat.us");

}).AddTypedClient<IHttpClient>((client) =>
{
    // Cria o cliente Refit com a configuração de Polly
    var httpClient = Refit.RestService.For<IHttpClient>(client);
    return httpClient;
}).AddPolicyHandler((serviceProvider, request) =>
    PollyResilienceApp.Policies.PollyConfigPolicyBuilder.Build(serviceProvider)
)
.AddTypedClient(p => Refit.RestService.For<IHttpClient>(p));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
