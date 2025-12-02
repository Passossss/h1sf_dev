using H1SF.Domain.Entities;
using H1SF.Domain.Entities.Faturamento;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Entidades principais
    public DbSet<Impressora> Impressoras { get; set; }
    public DbSet<MonitorFaturamento> MonitorFaturamento { get; set; }
    public DbSet<SelecaoFaturamento> SelecoesFaturamento { get; set; }
    public DbSet<ItemFaturado> ItensFaturados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar Impressora
        modelBuilder.Entity<Impressora>(entity =>
        {
            entity.HasKey(e => e.IdImpr);
            entity.ToTable("IMPRESSORA");
        });

        // Configurar MonitorFaturamento
        modelBuilder.Entity<MonitorFaturamento>(entity =>
        {
            entity.HasKey(e => new { e.CodigoMercadoriaDestino, e.TimestampSelecao, e.LoginFuncionario });
            entity.ToTable("MNT_MONITOR_FTRM");
        });

        // Configurar SelecaoFaturamento
        modelBuilder.Entity<SelecaoFaturamento>(entity =>
        {
            entity.HasKey(e => new { e.SftCdMercDst, e.SftDtcSelFtrm, e.SftLgonFunc });
            entity.ToTable("SFT_SEL_FTRM");
        });

        // Configurar ItemFaturado
        modelBuilder.Entity<ItemFaturado>(entity =>
        {
            entity.HasKey(e => new { e.ItdCdMercDst, e.ItdDtcSelFtrm, e.ItdLgonFunc });
            entity.ToTable("ITD_ITEM_FTRD");
        });
    }
}
