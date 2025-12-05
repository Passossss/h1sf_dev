using H1SF.Application.Services;
using H1SF.Application.Services.DataHora;
using H1SF.Application.Services.DreDetalhesRelatorio;
using H1SF.Application.Services.EntradaNfIcRis;
using H1SF.Infrastructure.Data;
using H1SF.Infrastructure.Repositories;
using H1SF.Infrastructure.Repositories.DataHora;
using H1SF.Infrastructure.Repositories.DreDetalhesRelatorio;
using H1SF.Infrastructure.Repositories.EntradaNfIcRis;
using H1SF.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Execution Started: " + DateTime.Now);

try { 
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register RotinaPrincipalBootstrap and its dependencies
        services.AddTransient<IDefinirImpressoraRepository, DefinirImpressoraRepository>();
        services.AddTransient<IEntradaRisRepository, EntradaRisServiceRisRepository>();
        services.AddTransient<IDetalheRelatorioRepository, DetalheRelatorioRepository>();
        services.AddTransient<IMonitorFaturamentoRepository, MonitorFaturamentoRepository>();
        services.AddTransient<IDataHoraRepository, DataHoraRepository>();        

        services.AddTransient<IImpressoraService, ImpressoraService>();
        services.AddTransient<IEntradaRisService, EntradaRisService>();
        services.AddTransient<IDetalheRelatorioService, DetalheRelatorioService>();
        services.AddTransient<IProcessadorFaturamento, ProcessadorFaturamento>();
        services.AddTransient<IProcessadorFaturamentoService, ProcessadorFaturamentoService>();
        services.AddTransient<IRecuperadorDataHora, RecuperadorDataHora>();
        services.AddTransient<IAtualizadorMonitor, AtualizadorMonitor>();

        services.AddTransient<IRotinaPrincipalBootstrap, RotinaPrincipalBootstrap>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
    })
    .Build();

using var scope = host.Services.CreateScope();
var rotinaPrincipal = scope.ServiceProvider.GetRequiredService<IRotinaPrincipalBootstrap>();

// Call the main routine
rotinaPrincipal.RotinaPrincipal();
}
catch (Exception ex)
{
    Console.WriteLine("Execution Error: " + DateTime.Now);
    Console.WriteLine(ex.Message);
    
}

Console.WriteLine("Execution Completed: " + DateTime.Now);