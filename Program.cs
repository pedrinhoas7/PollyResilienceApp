using PollyResilienceApp.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Adicionando a configuração PollySettings no container
builder.Services.Configure<PollySettings>(builder.Configuration.GetSection("Polly"));

// Configurando o HttpClient para usar as políticas do Polly
builder.Services.AddHttpClient("Client", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");

}).AddTypedClient(p => Refit.RestService.For<IHttpClient>(p))
    .AddPolicyHandler((serviceProvider, request) =>
        PollyResilienceApp.Policies.PollyConfigPolicyBuilder.Build(serviceProvider));


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
