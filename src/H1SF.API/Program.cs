using H1SF.Application.Services;
using H1SF.Application.Services.DataHora;
using H1SF.Application.Services.DreDetalhesRelatorio;
using H1SF.Application.Services.Emitente;
using H1SF.Application.Services.EntradaNfIcRis;
using H1SF.Application.Services.Fabrica;
using H1SF.Application.Services.FaturamentoPws;
using H1SF.Application.Services.LogCaps;
using H1SF.Application.Services.Memoria;
using H1SF.Application.Services.Monitor;
using H1SF.Application.Services.Protocolo;
using H1SF.Application.Services.Recolhimento;
using H1SF.Application.Services.Transacao;
using H1SF.Infrastructure.Data;
using H1SF.Infrastructure.Repositories;
using H1SF.Infrastructure.Repositories.DataHora;
using H1SF.Infrastructure.Repositories.DreDetalhesRelatorio;
using H1SF.Infrastructure.Repositories.Emitente;
using H1SF.Infrastructure.Repositories.EntradaNfIcRis;
using H1SF.Infrastructure.Repositories.Fabrica;
using H1SF.Infrastructure.Repositories.FaturamentoPws;
using H1SF.Infrastructure.Repositories.LogCaps;
using H1SF.Infrastructure.Repositories.Protocolo;
using H1SF.Infrastructure.Repositories.Recolhimento;
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

builder.Services.AddScoped<IDataHoraRepository, DataHoraRepository>();
builder.Services.AddScoped<IRecuperadorDataHora, RecuperadorDataHora>();

builder.Services.AddScoped<ICnpjFabricaRepository, CnpjFabricaRepository>();
builder.Services.AddScoped<IRecuperadorCnpjFabrica, RecuperadorCnpjFabrica>();

builder.Services.AddScoped<IEmitenteRepository, EmitenteRepository>();
builder.Services.AddScoped<IRecuperadorEmitente, RecuperadorEmitente>();

builder.Services.AddScoped<IProtocoloRepository, ProtocoloRepository>();
builder.Services.AddScoped<ILeitorProtocolo, LeitorProtocolo>();

builder.Services.AddScoped<IAtualizarPwsRepository, AtualizarPwsRepository>();
builder.Services.AddScoped<IAtualizarPwsService, AtualizarPwsService>();

builder.Services.AddScoped<ILogCapsRepository, LogCapsRepository>();
builder.Services.AddScoped<IMontadorLogCaps, MontadorLogCaps>();

builder.Services.AddScoped<IIniciadorTransacaoSf30, IniciadorTransacaoSf30>();

builder.Services.AddScoped<IAtualizadorFaseLbrcImps, AtualizadorFaseLbrcImps>();

builder.Services.AddScoped<IFinalizadorItemRecolhimentoRepository, FinalizadorItemRecolhimentoRepository>();
builder.Services.AddScoped<IFinalizadorItemRecolhimento, FinalizadorItemRecolhimento>();

builder.Services.AddScoped<IEmissorSyncpoint, EmissorSyncpoint>();

builder.Services.AddScoped<ILiberadorMemoria, LiberadorMemoria>();

builder.Services.AddScoped<IDetalheRelatorioService, DetalheRelatorioService>();
builder.Services.AddScoped<IDetalheRelatorioRepository, DetalheRelatorioRepository>();

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
