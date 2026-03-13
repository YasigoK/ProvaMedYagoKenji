using DesafioMed.Contatos.API.Configuration;
using DesafioMed.Contatos.API.Endpoints;
using DesafioMed.Contatos.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiVersioningAndSwagger();
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerByVersion();
app.UseHttpsRedirection();
app.MapContatoEndpoints();

app.Run();