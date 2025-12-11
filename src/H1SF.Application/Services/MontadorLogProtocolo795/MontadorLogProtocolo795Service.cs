using H1SF.Application.DTOs.MontadorLog;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services;

/// <summary>
/// 795-00-MONTA-LOG-PROTOCOLO
/// Autor: A.C.ANDREATTA
/// </summary>
public class MontadorLogProtocolo795Service : IMontadorLogProtocolo795
{
    private readonly ApplicationDbContext _context;

    public MontadorLogProtocolo795Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MontadorLogProtocolo795Output> ExecutarAsync(MontadorLogProtocolo795Input input)
    {
        var resultado = new MontadorLogProtocolo795Output
        {
            Sucesso = false
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '795-00-MONTA-LOG-PROTOCOLO' TO WS35-AUX-TS
        var auxTs = "795-00-MONTA-LOG-PROTOCOLO";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '01' TO ACB5022-CD-ACES
        resultado.CodigoAcesso = "01";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO ACB5022-CD-RETR-ECI
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO ACB5022-CD-RETR-ACES
        var codigoRetornoEci = 0;
        var codigoRetornoAcesso = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '03' TO ACB50221-CD-ACES-MQ
        var codigoAcessoMq = "03";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 00005 TO ACB50221-ID-FILA-MQ
        var idFilaMq = 5;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO ACB50221-CD-RETR-MQ
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO ACB50221-CD-COMP
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO ACB50221-CD-RSON
        var codigoRetornoMq = 0;
        var codigoComp = 0;
        var codigoReason = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO ACB50221-ID-MSG-PRC
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO ACB50221-ID-MSG-AUX
        var idMsgProcesso = string.Empty;
        var idMsgAuxiliar = string.Empty;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO ST8001-ST80015
        var st80015 = string.Empty;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '11' TO ST80015-CD-ACES
        var cdAces = "11";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-CD-T-REC TO ST80015-CD-T-REC
        var cdTipoRec = input.CodigoTipoRecolhimento;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-CD-T-MTZ TO ST80015-CD-T-MTZ
        var cdTipoMtz = input.CodigoTipoMatriz;

        // Busca modalidade transporte e ID cliente relacionado
        var dadosItem = await BuscarDadosItemAsync(input);

        if (dadosItem == null)
        {
            resultado.Sucesso = false;
            return resultado;
        }

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0002-ITD-CD-MOD-TRSP-LOG TO ST80015-CD-MOD-TRSP
        resultado.CodigoModalidadeTransporte = dadosItem.CodigoModalidadeTransporte;

        // Define matriz e cliente
        if (dadosItem.IdClienteRelacionado.HasValue)
        {
            var idMatrizControle = await BuscarMatrizControleVolumeAsync(dadosItem.IdClienteRelacionado.Value);

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE ST0008-ID-MTZ TO ST80015-ID-MTZ
            resultado.IdMatriz = idMatrizControle;

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE ST0005-ID-CLI-RLCD TO ST80015-ID-CLI
            resultado.IdCliente = dadosItem.IdClienteRelacionado.Value;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-MTZ TO ST80015-ID-MTZ
            resultado.IdMatriz = input.IdMatriz;

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-CLI TO ST80015-ID-CLI
            resultado.IdCliente = input.PtdIdCliente;
        }

        // Busca volume e fatura exportação
        var dadosVolume = await BuscarDadosVolumeAsync(input);

        if (dadosVolume == null)
        {
            resultado.Sucesso = false;
            return resultado;
        }

        // Busca ID agrupamento
        var idAgrupamento = await BuscarIdAgrupamentoAsync(input, dadosVolume.IdVolume);

        if (string.IsNullOrWhiteSpace(idAgrupamento))
        {
            resultado.Sucesso = false;
            return resultado;
        }

        //mock
        // Este trecho está mockado. Nome COBOL: INSPECT SF0005-SFT-ID-AGRP CONVERTING SPACES TO ZEROS
        idAgrupamento = idAgrupamento.Replace(" ", "0");

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0005-SFT-ID-AGRP TO ST80015-ID-AGRP
        resultado.IdAgrupamento = idAgrupamento;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-PTC-DSP TO ST80015-ID-PTC-DSP
        resultado.IdProtocoloDespacho = input.IdProtocoloDespacho;

        //mock
        // Este trecho está mockado. Nome COBOL: INSPECT SF0002-ITD-FTR-EXP-LOG CONVERTING SPACES TO ZEROS
        var faturaExp = dadosVolume.FaturaExportacao.Replace(" ", "0");

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0002-ITD-FTR-EXP-LOG TO ST80015-ID-FTR-EXP
        resultado.IdFaturaExportacao = faturaExp;

        // Define código pagamento frete
        if (input.CodigoPagamentoFrete == "1")
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 'R' TO ST80015-CD-PGT-FRT
            resultado.CodigoPagamentoFrete = "R";
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 'D' TO ST80015-CD-PGT-FRT
            resultado.CodigoPagamentoFrete = "D";
        }

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-CD-TRSR TO ST80015-CD-TRSR
        var codigoTransportador = input.CodigoTransportador;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE CC0005-RAZAO-SOCIAL-E TO ST80015-NM-TRSR
        var nomeTransportador = input.RazaoSocialTransportador;

        //mock
        // Este trecho está mockado. Nome COBOL: INSPECT SF0001-PTD-Q-TTL-VOL-LOG CONVERTING SPACES TO ZEROS
        var qtdVolumes = input.QuantidadeTotalVolumes.Replace(" ", "0");

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-Q-TTL-VOL-LOG TO ST80015-Q-VOL-PTC-DSP
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-PESO-TTL-LQD-LOG TO ST80015-V-TTL-PESO-LQD-R
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-PESO-TTL-EMB-LOG TO ST80015-V-TTL-PESO-EMB-R
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-PESO-TTL-BRT-LOG TO ST80015-V-TTL-PESO-BRT-R
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-V-TTL-MRCD-LOG TO ST80015-V-TTL-MRCD-DSP-R

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'BPIS' TO ST80015-ID-LGON-FUNC
        var idLogonFunc = "BPIS";

        // Envia mensagem se necessário
        if (input.FaseFaturamento == "1" && input.CodigoRegraFaturamento != "N")
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-CD-T-REC TO ACB50221-CHV-PRAD
            // Este trecho está mockado. Nome COBOL: MOVE 167 TO ACB50221-TAM-MSG
            // Este trecho está mockado. Nome COBOL: MOVE ST8001-ST80015 TO ACB50221-TXT-MSG
            // Este trecho está mockado. Nome COBOL: PERFORM 555-00-GRAVA-MENSAGEM-MQ
            resultado.MensagemEnviada = true;
        }

        resultado.Sucesso = true;
        return resultado;
    }

    private async Task<DadosItemFaturado?> BuscarDadosItemAsync(MontadorLogProtocolo795Input input)
    {
        // EXEC SQL SELECT A.ITD_CD_MOD_TRSP, B.ID_CLI_RLCD...
        var sql = @"
            SELECT A.ITD_CD_MOD_TRSP AS CodigoModalidadeTransporte,
                   B.ID_CLI_RLCD AS IdClienteRelacionado
            FROM H1SF.ITD_ITMFATURADO A
            INNER JOIN H1ST.ITEM_RECOLHIMENTO B ON A.ITD_ID_ETIQ_REC = B.ID_ETIQ_REC
            WHERE A.ITD_CD_MERC_DST = {0}
              AND A.ITD_DTC_SEL_FTRM = {1}
              AND A.ITD_LGON_FUNC = {2}
              AND A.ITD_ID_PTC_DSP = {3}
              AND A.ITD_ID_NF = {4}
              AND ROWNUM = 1";

        var dados = await _context.Database
            .SqlQueryRaw<DadosItemFaturado>(sql,
                input.CodigoMercadoriaDestino,
                input.DataSelecaoFaturamento,
                input.LoginFuncionario,
                input.IdProtocoloDespacho,
                input.NumeroNota)
            .FirstOrDefaultAsync();

        return dados;
    }

    private async Task<int> BuscarMatrizControleVolumeAsync(int idCliente)
    {
        // EXEC SQL SELECT ID_MTZ...
        var sql = @"
            SELECT ID_MTZ
            FROM H1ST.CONTROLE_VOLUME
            WHERE ID_CLI = {0}
              AND ROWNUM = 1";

        var idMatriz = await _context.Database
            .SqlQueryRaw<int>(sql, idCliente)
            .FirstOrDefaultAsync();

        return idMatriz;
    }

    private async Task<DadosVolume?> BuscarDadosVolumeAsync(MontadorLogProtocolo795Input input)
    {
        // EXEC SQL DECLARE CSR_SEL_795 CURSOR FOR...
        var sql = @"
            SELECT ITD_ID_VOL AS IdVolume,
                   ITD_FTR_EXP AS FaturaExportacao
            FROM H1SF.ITD_ITMFATURADO
            WHERE ITD_CD_MERC_DST = {0}
              AND ITD_DTC_SEL_FTRM = {1}
              AND ITD_LGON_FUNC = {2}
              AND ITD_ID_CLI = {3}
              AND ITD_ID_PTC_DSP = {4}
              AND ITD_ID_NF = {5}
              AND ROWNUM = 1";

        var dados = await _context.Database
            .SqlQueryRaw<DadosVolume>(sql,
                input.WqCodigoMercadoriaDestino,
                input.WqDataSelecaoFaturamento,
                input.WqLoginFuncionario,
                input.IdCliente,
                input.IdProtocoloDespacho,
                input.NumeroNota)
            .FirstOrDefaultAsync();

        return dados;
    }

    private async Task<string?> BuscarIdAgrupamentoAsync(MontadorLogProtocolo795Input input, string idVolume)
    {
        // EXEC SQL SELECT SFT_ID_AGRP...
        var sql = @"
            SELECT SFT_ID_AGRP
            FROM H1SF.SFT_SELECAO_FTRM
            WHERE SFT_CD_MERC_DST = {0}
              AND SFT_DTC_SEL_FTRM = {1}
              AND SFT_LGON_FUNC = {2}
              AND SFT_ID_CLI = {3}
              AND SFT_ID_VOL = {4}";

        var idAgrp = await _context.Database
            .SqlQueryRaw<string>(sql,
                input.WqCodigoMercadoriaDestino,
                input.WqDataSelecaoFaturamento,
                input.WqLoginFuncionario,
                input.IdCliente,
                idVolume)
            .FirstOrDefaultAsync();

        return idAgrp;
    }
}

public class DadosItemFaturado
{
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;
    public int? IdClienteRelacionado { get; set; }
}

public class DadosVolume
{
    public string IdVolume { get; set; } = string.Empty;
    public string FaturaExportacao { get; set; } = string.Empty;
}
