using H1SF.Domain.Entities;
using H1SF.Domain.Entities.Emitente;
using H1SF.Domain.Entities.Fabrica;
using H1SF.Domain.Entities.Faturamento;
using H1SF.Domain.Entities.LogCaps;
using H1SF.Domain.Entities.Protocolo;
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
    public DbSet<TipoRecolhimento> TiposRecolhimento { get; set; }
    
    // Entidades de Fábrica
    public DbSet<H1SF.Domain.Entities.Fabrica.Fabrica> Fabricas { get; set; }
    public DbSet<ControleVolume> ControlesVolume { get; set; }
    
    // Entidades de Emitente
    public DbSet<CorPessoa> CorPessoas { get; set; }
    public DbSet<CorLocalidadeVigencia> CorLocalidadeVigencias { get; set; }
    public DbSet<CorMunicipio> CorMunicipios { get; set; }
    public DbSet<CorPessoaVigencia> CorPessoaVigencias { get; set; }
    
    // Entidades de Protocolo
    public DbSet<ProtocoloDespacho> ProtocolosDespacho { get; set; }
    
    // Entidades de Recolhimento (PWS)
    public DbSet<ItemRecolhimento> ItensRecolhimento { get; set; }
    public DbSet<VolumeRecolhimento> VolumesRecolhimento { get; set; }
    public DbSet<ItemVolumeRecolhimento> ItensVolumeRecolhimento { get; set; }
    
    // Entidades de Log CAPS
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Peca> Pecas { get; set; }

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

        // Configurar TipoRecolhimento
        modelBuilder.Entity<TipoRecolhimento>(entity =>
        {
            entity.HasKey(e => e.CdTRec);
            entity.ToTable("TIPO_RECOLHIMENTO", schema: "H1ST");
        });

        // Configurar Fabrica
        modelBuilder.Entity<H1SF.Domain.Entities.Fabrica.Fabrica>(entity =>
        {
            entity.HasKey(e => e.CdFbr);
            entity.ToTable("FABRICA", schema: "H1CB");
        });

        // Configurar ControleVolume
        modelBuilder.Entity<ControleVolume>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("CONTROLE_VOLUME", schema: "H1ST");
        });

        // Configurar CorPessoa
        modelBuilder.Entity<CorPessoa>(entity =>
        {
            entity.HasKey(e => e.PfjCodigo);
            entity.ToTable("COR_PESSOA", schema: "B8CC");
        });

        // Configurar CorLocalidadeVigencia
        modelBuilder.Entity<CorLocalidadeVigencia>(entity =>
        {
            entity.HasKey(e => new { e.PfjCodigo, e.LocCodigo });
            entity.ToTable("COR_LOCALIDADE_VIGENCIA", schema: "B8CC");
        });

        // Configurar CorMunicipio
        modelBuilder.Entity<CorMunicipio>(entity =>
        {
            entity.HasKey(e => e.MunCodigo);
            entity.ToTable("COR_MUNICIPIO", schema: "B8CC");
        });

        // Configurar CorPessoaVigencia
        modelBuilder.Entity<CorPessoaVigencia>(entity =>
        {
            entity.HasKey(e => e.PfjCodigo);
            entity.ToTable("COR_PESSOA_VIGENCIA", schema: "B8CC");
        });

        // Configurar ProtocoloDespacho
        modelBuilder.Entity<ProtocoloDespacho>(entity =>
        {
            entity.HasKey(e => new { e.PtdCdMercDst, e.PtdDtcSelFtrm, e.PtdLgonFunc, e.PtdCdSeq });
            entity.ToTable("PTD_PROTODSP", schema: "H1SF");
        });

        // Configurar ItemRecolhimento
        modelBuilder.Entity<ItemRecolhimento>(entity =>
        {
            entity.HasKey(e => e.IdEtiqRec);
            entity.ToTable("ITEM_RECOLHIMENTO", schema: "H1ST");
        });

        // Configurar VolumeRecolhimento
        modelBuilder.Entity<VolumeRecolhimento>(entity =>
        {
            entity.HasKey(e => e.IdVol);
            entity.ToTable("VOLUME_RECOLHIMENTO", schema: "H1ST");
        });

        // Configurar ItemVolumeRecolhimento
        modelBuilder.Entity<ItemVolumeRecolhimento>(entity =>
        {
            entity.HasKey(e => new { e.IdEtiqRec, e.IdVol });
            entity.ToTable("ITEM_VOLUME_RECOLHIMENTO", schema: "H1ST");
        });

        // Configurar Fornecedor
        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => e.CodigoFonteAtendimento);
            entity.ToTable("SUP_FORNECEDOR", schema: "H1SF");
            
            entity.Property(e => e.CodigoFonteAtendimento)
                .HasColumnName("SUP_CD_FNT_ATND");
            entity.Property(e => e.CodigoFornecedorSuprimentos)
                .HasColumnName("SUP_CD_FORN_SPR");
        });

        // Configurar Peca
        modelBuilder.Entity<Peca>(entity =>
        {
            entity.HasKey(e => e.IdPeca);
            entity.ToTable("PECA", schema: "H1SR");
            
            entity.Property(e => e.IdPeca)
                .HasColumnName("ID_PECA");
            entity.Property(e => e.NomePecaIngles)
                .HasColumnName("NM_PECA_ING");
        });
    }
}
