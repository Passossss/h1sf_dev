using H1SF.Application.DTOs.MontadorPckList;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services;

/// <summary>
/// 720-00-MONTA-PCK-LIST
/// Autor: E. FRIOLI JR.
/// </summary>
public class MontadorPckList720Service : IMontadorPckList720
{
    private readonly ApplicationDbContext _context;

    public MontadorPckList720Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MontadorPckList720Output> ExecutarAsync(MontadorPckList720Input input)
    {
        var resultado = new MontadorPckList720Output
        {
            Sucesso = false,
            VolumesProcessados = 0,
            LinhasGeradas = 0
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '720-00-MONTA-PCK-LIST' TO WS35-AUX-TS
        var auxTs = "720-00-MONTA-PCK-LIST";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'N' TO WS31-CHV-FIM-PCK-LST
        var chaveFimPckLst = "N";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO WS01-ID-VOL-PCK-LST-ANT
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO WS01-ID-PDD-PCK-LST-ANT
        var idVolPckLstAnterior = string.Empty;
        var idPddPckLstAnterior = string.Empty;

        //mock
        // Este trecho está mockado. Nome COBOL: ADD 1 TO WS01-NUM-VIAS-VOL
        var numeroViasVolume = input.NumeroViasVolume + 1;

        // Calcula índice PJL
        var indicePjlPckLstAux = CalcularIndicePjl(
            numeroViasVolume,
            input.CodigoMercadoriaDestino,
            input.CodigoModalidadeTransporte);

        //mock
        // Este trecho está mockado. Nome COBOL: ADD WS01-IND-PJL-PKLST-AUX TO WS01-CD-SQN-PJL-PKLST-N
        var codigoSequenciaPjl = input.CodigoSequenciaPjlPckLst + indicePjlPckLstAux;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-CD-SQN-PJL-PKLST TO CB0002-DRE-CD-SQN-PJL
        var dreCodigoSequenciaPjl = codigoSequenciaPjl;

        //mock
        // Este trecho está mockado. Nome COBOL: SUBTRACT WS01-IND-PJL-PKLST-AUX FROM WS01-CD-SQN-PJL-PKLST-N
        codigoSequenciaPjl -= indicePjlPckLstAux;

        // 720-05-DEFINE-IDIOMA
        resultado.IdIdioma = await DefinirIdiomaAsync(input.FaturaExportacao);

        // 720-07-DEFINE-TITULO
        if (input.ChaveDea == "S")
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE '00334' TO SR0002-ID-PARM
            // Este trecho está mockado. Nome COBOL: MOVE '2600' TO WS01-POS-PACK-LIST-X
            resultado.PosicaoPackListX = "2600";
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE '00333' TO SR0002-ID-PARM
            // Este trecho está mockado. Nome COBOL: MOVE '2140' TO WS01-POS-PACK-LIST-X
            resultado.PosicaoPackListX = "2140";
        }

        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 530-00-RECUPERA-PARAMETRO
        // Este trecho está mockado. Nome COBOL: MOVE SR0002-CN-PARM TO WS01-TITULO-PACK-LIST
        resultado.TituloPackList = input.ParametroTitulo;

        // 720-10-MONTA-CORPO-INICIO
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
        await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
        resultado.LinhasGeradas++;

        // 720-20-DADOS-DE-DETALHE
        var itens = await BuscarItensAsync(input);

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO WS01-SEQ-VOL-PTD
        var sequenciaVolume = 0;

        // 720-30-LOOP-DET-VOL
        foreach (var item in itens)
        {
            // 720-40-VERIFICA-CAB-QUEBRA
            if (item.IdVolume != idVolPckLstAnterior || chaveFimPckLst == "S")
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE SPACES TO WS01-ID-PDD-PCK-LST-ANT
                idPddPckLstAnterior = string.Empty;

                if (!string.IsNullOrWhiteSpace(idVolPckLstAnterior))
                {
                    sequenciaVolume++;

                    if (sequenciaVolume > 48)
                    {
                        sequenciaVolume = 1;
                        //mock
                        // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
                        await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
                        resultado.LinhasGeradas++;
                    }

                    //mock
                    // Este trecho está mockado. Nome COBOL: COMPUTE WS01-CALC-POS-AUX = 888 + ((WS01-SEQ-VOL-PTD - 1) * 75)
                    var posicaoY = 888 + ((sequenciaVolume - 1) * 75);

                    // Busca totais do volume anterior
                    var totaisVolume = await BuscarTotaisVolumeAsync(input, idVolPckLstAnterior);

                    // Imprime totais do volume
                    resultado.LinhasGeradas += await ImprimirTotaisVolumeAsync(
                        totaisVolume, 
                        idVolPckLstAnterior, 
                        resultado.IdIdioma,
                        posicaoY);

                    resultado.VolumesProcessados++;
                }

                if (!string.IsNullOrWhiteSpace(idVolPckLstAnterior) && chaveFimPckLst == "S")
                {
                    sequenciaVolume++;

                    if (sequenciaVolume > 48)
                    {
                        sequenciaVolume = 1;
                        //mock
                        // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
                        await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
                        resultado.LinhasGeradas++;
                    }

                    var posicaoY = 888 + ((sequenciaVolume - 1) * 75);

                    // Busca totais gerais
                    var totaisGerais = await BuscarTotaisGeraisAsync(input);

                    // Imprime totais gerais
                    resultado.LinhasGeradas += await ImprimirTotaisGeraisAsync(
                        totaisGerais,
                        input.FaturaExportacao,
                        resultado.IdIdioma,
                        posicaoY);

                    break;
                }

                sequenciaVolume++;

                if (sequenciaVolume > 46)
                {
                    sequenciaVolume = 1;
                    //mock
                    // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
                    await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
                    resultado.LinhasGeradas++;
                }

                var posY = 888 + ((sequenciaVolume - 1) * 75);

                // Imprime cabeçalho do volume
                resultado.LinhasGeradas += await ImprimirCabecalhoVolumeAsync(item, resultado.IdIdioma, posY);

                //mock
                // Este trecho está mockado. Nome COBOL: MOVE SF0002-ITD-ID-VOL TO WS01-ID-VOL-PCK-LST-ANT
                idVolPckLstAnterior = item.IdVolume;
            }

            // Verifica quebra por PO
            if (item.IdPdd != idPddPckLstAnterior)
            {
                sequenciaVolume++;

                if (sequenciaVolume > 47)
                {
                    sequenciaVolume = 1;
                    //mock
                    // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
                    await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
                    resultado.LinhasGeradas++;
                }

                var posY = 888 + ((sequenciaVolume - 1) * 75);

                // Imprime PO
                resultado.LinhasGeradas += await ImprimirPoAsync(item.IdPdd, posY);

                //mock
                // Este trecho está mockado. Nome COBOL: MOVE SF0002-ITD-ID-PDD TO WS01-ID-PDD-PCK-LST-ANT
                idPddPckLstAnterior = item.IdPdd;
            }

            // 720-50-IMPRIME-DETALHE
            sequenciaVolume++;

            if (sequenciaVolume > 48)
            {
                sequenciaVolume = 1;
                //mock
                // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
                await MontarCorpoPackListAsync(resultado.IdIdioma, resultado.TituloPackList, resultado.PosicaoPackListX);
                resultado.LinhasGeradas++;
            }

            var posicaoYDetalhe = 888 + ((sequenciaVolume - 1) * 75);

            // Imprime detalhe do item
            resultado.LinhasGeradas += await ImprimirDetalheItemAsync(item, posicaoYDetalhe);
        }

        // 720-60-CLOSE-DET-VOL (cursor fechado automaticamente)

        resultado.Sucesso = true;
        return resultado;
    }

    private int CalcularIndicePjl(int numeroViasVolume, string codigoMercadoriaDestino, string codigoModalidadeTransporte)
    {
        if (numeroViasVolume == 1)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 1 TO WS01-IND-PJL-PKLST-AUX
            return 1;
        }
        else if (numeroViasVolume == 2)
        {
            if (codigoMercadoriaDestino == "E" &&
                (codigoModalidadeTransporte == "1" ||
                 codigoModalidadeTransporte == "2" ||
                 codigoModalidadeTransporte == "3"))
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 21 TO WS01-IND-PJL-PKLST-AUX
                return 21;
            }
            else
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 3 TO WS01-IND-PJL-PKLST-AUX
                return 3;
            }
        }
        else if (numeroViasVolume == 3)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 5 TO WS01-IND-PJL-PKLST-AUX
            return 5;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 7 TO WS01-IND-PJL-PKLST-AUX
            return 7;
        }
    }

    private async Task<string> DefinirIdiomaAsync(string faturaExportacao)
    {
        // EXEC SQL SELECT ID_IDM...
        var sql = @"
            SELECT ID_IDM
            FROM H1SR.PRECO_FATURA_API
            WHERE ID_FTR = {0}
              AND ROWNUM = 1";

        var idioma = await _context.Database
            .SqlQueryRaw<string>(sql, faturaExportacao)
            .FirstOrDefaultAsync();

        //mock
        // Este trecho está mockado. Nome COBOL: IF SQLCODE EQUAL WS31-CHV-NTFD-SQL OR SR0005-ID-IDM NOT GREATER SPACES OR WI01-ID-IDM EQUAL -1
        if (string.IsNullOrWhiteSpace(idioma))
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 'E' TO SR0005-ID-IDM
            return "E";
        }

        return idioma;
    }

    private async Task MontarCorpoPackListAsync(string idioma, string titulo, string posicaoX)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 725-00-MONTA-CORPO-PCK-LIST
        await Task.CompletedTask;
    }

    private async Task<List<ItemPackList720>> BuscarItensAsync(MontadorPckList720Input input)
    {
        // EXEC SQL DECLARE CSR_SEL_720 CURSOR FOR...
        var sql = @"
            SELECT A.ITD_ID_VOL AS IdVolume,
                   D.ID_VOL_CMIS2 AS IdVolumeCmis2,
                   A.ITD_ID_PDD AS IdPdd,
                   TO_CHAR(A.ITD_Q_PECA_FTRD,'999999990') AS Quantidade,
                   SUBSTR(A.ITD_ID_PECA,1,3) || '-' || SUBSTR(A.ITD_ID_PECA,4,17) AS IdPeca,
                   C.CD_PFO AS CodigoPfo,
                   A.ITD_NM_PECA AS NomePeca,
                   A.ITD_SRIE_PECA AS SeriePeca,
                   B.CLSF_FSCL AS ClassificacaoFiscal,
                   TO_CHAR(A.ITD_PREC_US_API_UNT/100,'9,999,990.00') AS PrecoUnitario,
                   TO_CHAR(A.ITD_Q_PECA_FTRD*A.ITD_PREC_US_API_UNT/100,'999,999,990.00') AS PrecoTotal,
                   TO_CHAR(A.ITD_Q_PECA_FTRD*A.ITD_PESO_LQD_PECA/1000,'9,999,990.000') AS PesoTotal
            FROM H1SF.ITD_ITMFATURADO A
            INNER JOIN H1SR.PECA B ON A.ITD_ID_PECA = B.ID_PECA
            INNER JOIN H1ST.ITEM_RECOLHIMENTO C ON A.ITD_ID_ETIQ_REC = C.ID_ETIQ_REC
            INNER JOIN H1ST.VOLUME_RECOLHIMENTO D ON A.ITD_ID_VOL = D.ID_VOL
            WHERE A.ITD_CD_MERC_DST = {0}
              AND A.ITD_DTC_SEL_FTRM = {1}
              AND A.ITD_LGON_FUNC = {2}
              AND A.ITD_ID_PTC_DSP = {3}
            ORDER BY A.ITD_ID_VOL, A.ITD_ID_PDD, A.ITD_ID_PECA, C.CD_PFO";

        var itens = await _context.Database
            .SqlQueryRaw<ItemPackList720>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho)
            .ToListAsync();

        return itens;
    }

    private async Task<TotaisVolume720> BuscarTotaisVolumeAsync(MontadorPckList720Input input, string idVolume)
    {
        // EXEC SQL SELECT totais por volume
        var sql = @"
            SELECT TO_CHAR(SUM(ITD_Q_PECA_FTRD),'999999990') AS QuantidadeTotal,
                   TO_CHAR(SUM(ITD_Q_PECA_FTRD*ITD_PREC_US_API_UNT/100),'999,999,990.00') AS PrecoTotal,
                   TO_CHAR(SUM(ITD_Q_PECA_FTRD*ITD_PESO_LQD_PECA/1000),'9,999,990.000') AS PesoTotal
            FROM H1SF.ITD_ITMFATURADO
            WHERE ITD_CD_MERC_DST = {0}
              AND ITD_DTC_SEL_FTRM = {1}
              AND ITD_LGON_FUNC = {2}
              AND ITD_ID_PTC_DSP = {3}
              AND ITD_ID_VOL = {4}";

        var totais = await _context.Database
            .SqlQueryRaw<TotaisVolume720>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho,
                idVolume)
            .FirstOrDefaultAsync();

        return totais ?? new TotaisVolume720();
    }

    private async Task<TotaisVolume720> BuscarTotaisGeraisAsync(MontadorPckList720Input input)
    {
        // EXEC SQL SELECT totais gerais
        var sql = @"
            SELECT TO_CHAR(SUM(ITD_Q_PECA_FTRD),'999999990') AS QuantidadeTotal,
                   TO_CHAR(SUM(ITD_Q_PECA_FTRD*ITD_PREC_US_API_UNT/100),'999,999,990.00') AS PrecoTotal,
                   TO_CHAR(SUM(ITD_Q_PECA_FTRD*ITD_PESO_LQD_PECA/1000),'9,999,990.000') AS PesoTotal
            FROM H1SF.ITD_ITMFATURADO
            WHERE ITD_CD_MERC_DST = {0}
              AND ITD_DTC_SEL_FTRM = {1}
              AND ITD_LGON_FUNC = {2}
              AND ITD_ID_PTC_DSP = {3}";

        var totais = await _context.Database
            .SqlQueryRaw<TotaisVolume720>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho)
            .FirstOrDefaultAsync();

        return totais ?? new TotaisVolume720();
    }

    private async Task<int> ImprimirCabecalhoVolumeAsync(ItemPackList720 item, string idioma, int posicaoY)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(19U(s16901t3b0s10.00v1P' TO WS01-LINHA-IMPR-AUX
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await GravarDetalheRelatorioAsync("(19U(s16901t3b0s10.00v1P");
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '1832' TO WS01-PCL-POSICAO-X
        var labelCase = idioma == "E" ? "CASE:" : "BULTO:";
        await GravarDetalheRelatorioAsync(labelCase);
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '2170' TO WS01-PCL-POSICAO-X
        var idVolumeExibir = item.IdVolumeCmis2 ?? item.IdVolume;
        await GravarDetalheRelatorioAsync(idVolumeExibir);
        linhas++;

        return linhas;
    }

    private async Task<int> ImprimirPoAsync(string idPdd, int posicaoY)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(19U(s16901t3b0s10.00v1P' TO WS01-LINHA-IMPR-AUX
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await GravarDetalheRelatorioAsync("(19U(s16901t3b0s10.00v1P");
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0686' TO WS01-PCL-POSICAO-X
        // Este trecho está mockado. Nome COBOL: MOVE 'P.O.:' TO WS01-PCL-DADOS
        await GravarDetalheRelatorioAsync("P.O.:");
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0905' TO WS01-PCL-POSICAO-X
        // Este trecho está mockado. Nome COBOL: MOVE SF0002-ITD-ID-PDD TO WS01-PCL-DADOS
        await GravarDetalheRelatorioAsync(idPdd);
        linhas++;

        return linhas;
    }

    private async Task<int> ImprimirDetalheItemAsync(ItemPackList720 item, int posicaoY)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(s0p17.14h0s3b4099T' TO WS01-LINHA-IMPR-AUX
        await GravarDetalheRelatorioAsync("(s0p17.14h0s3b4099T");
        linhas++;

        // Imprime todos os campos do item
        await GravarDetalheRelatorioAsync(item.Quantidade);
        await GravarDetalheRelatorioAsync(item.IdPeca);
        await GravarDetalheRelatorioAsync(item.CodigoPfo ?? string.Empty);
        await GravarDetalheRelatorioAsync(item.NomePeca);
        await GravarDetalheRelatorioAsync(item.SeriePeca ?? string.Empty);
        await GravarDetalheRelatorioAsync(item.ClassificacaoFiscal ?? string.Empty);
        await GravarDetalheRelatorioAsync(item.PrecoUnitario);
        await GravarDetalheRelatorioAsync(item.PrecoTotal);
        await GravarDetalheRelatorioAsync(FormatarNumero(item.PesoTotal));
        linhas += 9;

        return linhas;
    }

    private async Task<int> ImprimirTotaisVolumeAsync(TotaisVolume720 totais, string idVolume, string idioma, int posicaoY)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(s0p12.00h0s3b4099T' TO WS01-LINHA-IMPR-AUX
        await GravarDetalheRelatorioAsync("(s0p12.00h0s3b4099T");
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0060' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(totais.QuantidadeTotal);
        linhas++;

        var labelCase = idioma == "E" ? " CASE    - " : " BULTO   - ";
        var textoTotal = $"<---------------------------  TOTAL{labelCase}{idVolume} --------------------------------->";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0675' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(textoTotal);
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '5160' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(totais.PrecoTotal);
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '5930' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.PesoTotal));
        linhas++;

        return linhas;
    }

    private async Task<int> ImprimirTotaisGeraisAsync(TotaisVolume720 totais, string faturaExp, string idioma, int posicaoY)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(s0p12.00h0s3b4099T' TO WS01-LINHA-IMPR-AUX
        await GravarDetalheRelatorioAsync("(s0p12.00h0s3b4099T");
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0060' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(totais.QuantidadeTotal);
        linhas++;

        var textoTotal = $"<---------------------------  TOTAL CNTRL   - {faturaExp} --------------------------------->";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0675' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(textoTotal);
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '5160' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(totais.PrecoTotal);
        linhas++;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '5930' TO WS01-PCL-POSICAO-X
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.PesoTotal));
        linhas++;

        return linhas;
    }

    private string FormatarNumero(string numero)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-PESO-PCK-LIST-TOT CONVERTING '.' TO '!'
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-PESO-PCK-LIST-TOT CONVERTING ',' TO '.'
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-PESO-PCK-LIST-TOT CONVERTING '!' TO ','
        return numero
            .Replace('.', '!')
            .Replace(',', '.')
            .Replace('!', ',');
    }

    private async Task GravarDetalheRelatorioAsync(string linha)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await Task.CompletedTask;
    }
}

public class ItemPackList720
{
    public string IdVolume { get; set; } = string.Empty;
    public string? IdVolumeCmis2 { get; set; }
    public string IdPdd { get; set; } = string.Empty;
    public string Quantidade { get; set; } = string.Empty;
    public string IdPeca { get; set; } = string.Empty;
    public string? CodigoPfo { get; set; }
    public string NomePeca { get; set; } = string.Empty;
    public string? SeriePeca { get; set; }
    public string? ClassificacaoFiscal { get; set; }
    public string PrecoUnitario { get; set; } = string.Empty;
    public string PrecoTotal { get; set; } = string.Empty;
    public string PesoTotal { get; set; } = string.Empty;
}

public class TotaisVolume720
{
    public string QuantidadeTotal { get; set; } = string.Empty;
    public string PrecoTotal { get; set; } = string.Empty;
    public string PesoTotal { get; set; } = string.Empty;
}
