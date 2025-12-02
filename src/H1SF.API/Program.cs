using H1SF.Application.Services;
using H1SF.Infrastructure.Data;
using H1SF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=localhost;Database=H1SF;Integrated Security=true;TrustServerCertificate=true"));

builder.Services.AddScoped<IProcessadorFaturamento, ProcessadorFaturamento>();
builder.Services.AddScoped<IAtualizadorMonitor, AtualizadorMonitor>();
builder.Services.AddScoped<IProcessadorFaturamentoService, ProcessadorFaturamentoService>();

builder.Services.AddScoped<IImpressoraService, ImpressoraService>();
builder.Services.AddScoped<IDefinirImpressoraRepository, DefinirImpressoraRepository>();

builder.Services.AddScoped<IMonitorFaturamentoRepository, MonitorFaturamentoRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "H1SF API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
