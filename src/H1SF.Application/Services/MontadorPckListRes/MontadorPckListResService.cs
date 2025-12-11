using H1SF.Application.DTOs.MontadorPckList;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services;

/// <summary>
/// 730-00-MONTA-PCK-LIST-RES
/// Autor: E. FRIOLI JR.
/// </summary>
public class MontadorPckListResService : IMontadorPckListRes
{
    private readonly ApplicationDbContext _context;

    public MontadorPckListResService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MontadorPckListResOutput> ExecutarAsync(MontadorPckListResInput input)
    {
        var resultado = new MontadorPckListResOutput
        {
            Sucesso = false,
            VolumesProcessados = 0,
            LinhasGeradas = 0
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '730-00-MONTA-PCK-LIST-RES' TO WS35-AUX-TS
        var auxTs = "730-00-MONTA-PCK-LIST-RES";

        //mock
        // Este trecho está mockado. Nome COBOL: ADD 1 TO WS01-NUM-VIAS-VOL
        var numeroViasVolume = input.NumeroViasVolume + 1;

        // Calcula índice PJL baseado no número de vias
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

        // 730-10-MONTA-CORPO-INICIO
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 735-00-MONTA-CORPO-PCK-LIST-RE
        await MontarCorpoPackListAsync(input);
        resultado.LinhasGeradas++;

        // 730-20-DADOS-DE-DETALHE
        var volumes = await BuscarVolumesAsync(input);

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO WS01-SEQ-VOL-PTD
        var sequenciaVolume = 0;

        // 730-30-LOOP-DET-VOL
        foreach (var volume in volumes)
        {
            sequenciaVolume++;

            // 730-40-IMPRIME-DETALHE
            if (sequenciaVolume > 40)
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 1 TO WS01-SEQ-VOL-PTD
                sequenciaVolume = 1;

                //mock
                // Este trecho está mockado. Nome COBOL: PERFORM 735-00-MONTA-CORPO-PCK-LIST-RE
                await MontarCorpoPackListAsync(input);
                resultado.LinhasGeradas++;
            }

            //mock
            // Este trecho está mockado. Nome COBOL: COMPUTE WS01-CALC-POS-AUX = 888 + ((WS01-SEQ-VOL-PTD - 1) * 75)
            var posicaoY = 888 + ((sequenciaVolume - 1) * 75);

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE WS01-CALC-POS-AUX TO WS01-PCL-POSICAO-Y
            var pclPosicaoY = posicaoY.ToString();

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE '(s0p12.00h0s3b4099T' TO WS01-LINHA-IMPR-AUX
            // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
            await GravarDetalheRelatorioAsync("(s0p12.00h0s3b4099T");
            resultado.LinhasGeradas++;

            // Imprime campos do volume (ID, acondicionamento, dimensões, pesos)
            resultado.LinhasGeradas += await ImprimirDetalhesVolumeAsync(volume, input.IdIdioma);

            resultado.VolumesProcessados++;
        }

        // 730-50-CLOSE-DET-VOL (cursor fechado automaticamente)

        // 730-60-MONTA-TOTAL-RES
        var totais = await BuscarTotaisAsync(input);
        resultado.LinhasGeradas += await ImprimirTotaisAsync(totais, input);

        // 730-70-LOOP-DET-TOT
        var tiposAcondicionamento = await BuscarTiposAcondicionamentoAsync(input);
        resultado.LinhasGeradas += await ImprimirTiposAcondicionamentoAsync(tiposAcondicionamento, input.IdIdioma);

        // 730-80-CLOSE-DET-TOT (cursor fechado automaticamente)

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

    private async Task MontarCorpoPackListAsync(MontadorPckListResInput input)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 735-00-MONTA-CORPO-PCK-LIST-RE
        // Montagem do corpo do relatório

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'E&f1010y2X' TO WS01-LINHA-IMPR-AUX
        await GravarDetalheRelatorioAsync("E&f1010y2X");

        if (input.IdIdioma == "E")
        {
            // 735-30-CORPO-INGLES
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE '(19U(s16901t3b0s16.00v1P' TO WS01-LINHA-IMPR-AUX
            await GravarDetalheRelatorioAsync("(19U(s16901t3b0s16.00v1P");
        }
        else
        {
            // 735-20-CORPO-ESPANHOL
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE '(19U(s16901t3b0s16.00v1P' TO WS01-LINHA-IMPR-AUX
            await GravarDetalheRelatorioAsync("(19U(s16901t3b0s16.00v1P");
        }

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-TITULO-PACK-LIST TO WS01-PCL-DADOS
        await GravarDetalheRelatorioAsync(input.TituloPackList);
    }

    private async Task<List<VolumePackList>> BuscarVolumesAsync(MontadorPckListResInput input)
    {
        // EXEC SQL DECLARE CSR_SEL_730 CURSOR FOR...
        var sql = @"
            SELECT E.ITD_ID_VOL AS IdVolume,
                   C.ID_VOL_CMIS2 AS IdVolumeCmis2,
                   D.NM_ACND_ING AS NomeAcondicionamentoIngles,
                   D.NM_ACND_ESPN AS NomeAcondicionamentoEspanhol,
                   C.ALT_VOL AS AlturaVolume,
                   C.CMP_VOL AS ComprimentoVolume,
                   C.LRG_VOL AS LarguraVolume,
                   TO_CHAR(TRUNC(C.ALT_VOL/100*C.CMP_VOL/100*C.LRG_VOL/100,3),'990.000') AS MetrosCubicos,
                   TO_CHAR(C.PESO_BRT_VOL/1000,'9,999,990.000') AS PesoTotal,
                   TO_CHAR((C.PESO_BRT_VOL-E.PESO_ITEM)/1000,'9,999,990.000') AS PesoTara,
                   TO_CHAR(E.PESO_ITEM/1000,'9,999,990.000') AS PesoLiquido
            FROM (
                SELECT A.ITD_ID_VOL,
                       SUM(A.ITD_Q_PECA_FTRD * A.ITD_PESO_LQD_PECA) AS PESO_ITEM
                FROM H1SF.ITD_ITMFATURADO A
                WHERE A.ITD_CD_MERC_DST = {0}
                  AND A.ITD_DTC_SEL_FTRM = {1}
                  AND A.ITD_LGON_FUNC = {2}
                  AND A.ITD_ID_PTC_DSP = {3}
                GROUP BY A.ITD_ID_VOL
            ) E
            INNER JOIN H1ST.VOLUME_RECOLHIMENTO C ON E.ITD_ID_VOL = C.ID_VOL
            INNER JOIN H1ST.TIPO_ACONDICIONAMENTO D ON C.CD_T_ACND = D.CD_T_ACND
            ORDER BY C.ID_VOL";

        var volumes = await _context.Database
            .SqlQueryRaw<VolumePackList>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho)
            .ToListAsync();

        return volumes;
    }

    private async Task<TotaisPackList> BuscarTotaisAsync(MontadorPckListResInput input)
    {
        // EXEC SQL SELECT totais...
        var sql = @"
            SELECT TO_CHAR(SUM(TRUNC(C.ALT_VOL/100*C.CMP_VOL/100*C.LRG_VOL/100,3)),'990.000') AS MetrosCubicos,
                   TO_CHAR(SUM(C.PESO_BRT_VOL)/1000,'9,999,990.000') AS PesoTotal,
                   TO_CHAR(SUM(C.PESO_BRT_VOL-D.PESO_ITEM)/1000,'9,999,990.000') AS PesoTara,
                   TO_CHAR(SUM(D.PESO_ITEM)/1000,'9,999,990.000') AS PesoLiquido
            FROM (
                SELECT A.ITD_ID_VOL,
                       SUM(A.ITD_Q_PECA_FTRD * A.ITD_PESO_LQD_PECA) AS PESO_ITEM
                FROM H1SF.ITD_ITMFATURADO A
                WHERE A.ITD_CD_MERC_DST = {0}
                  AND A.ITD_DTC_SEL_FTRM = {1}
                  AND A.ITD_LGON_FUNC = {2}
                  AND A.ITD_ID_PTC_DSP = {3}
                GROUP BY A.ITD_ID_VOL
            ) D
            INNER JOIN H1ST.VOLUME_RECOLHIMENTO C ON D.ITD_ID_VOL = C.ID_VOL";

        var totais = await _context.Database
            .SqlQueryRaw<TotaisPackList>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho)
            .FirstOrDefaultAsync();

        return totais ?? new TotaisPackList();
    }

    private async Task<List<TipoAcondicionamentoCount>> BuscarTiposAcondicionamentoAsync(MontadorPckListResInput input)
    {
        // EXEC SQL DECLARE CSR_SEL_730_T CURSOR FOR...
        var sql = @"
            SELECT MAX(C.NM_ACND_ING) || ' = ' || COUNT(B.ID_VOL) AS NomeIngles,
                   MAX(C.NM_ACND_ESPN) || ' = ' || COUNT(B.ID_VOL) AS NomeEspanhol
            FROM H1ST.VOLUME_RECOLHIMENTO B
            INNER JOIN H1ST.TIPO_ACONDICIONAMENTO C ON B.CD_T_ACND = C.CD_T_ACND
            WHERE EXISTS (
                SELECT 1
                FROM H1SF.ITD_ITMFATURADO A
                WHERE A.ITD_ID_VOL = B.ID_VOL
                  AND A.ITD_CD_MERC_DST = {0}
                  AND A.ITD_DTC_SEL_FTRM = {1}
                  AND A.ITD_LGON_FUNC = {2}
                  AND A.ITD_ID_PTC_DSP = {3}
                  AND ROWNUM = 1
            )
            GROUP BY C.CD_T_ACND
            ORDER BY C.CD_T_ACND";

        var tipos = await _context.Database
            .SqlQueryRaw<TipoAcondicionamentoCount>(sql,
                input.PtdCodigoMercadoriaDestino,
                input.PtdDataSelecaoFaturamento,
                input.PtdLoginFuncionario,
                input.PtdIdProtocoloDespacho)
            .ToListAsync();

        return tipos;
    }

    private async Task<int> ImprimirDetalhesVolumeAsync(VolumePackList volume, string idioma)
    {
        var linhas = 0;

        // Imprime ID do volume
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0143' TO WS01-PCL-POSICAO-X
        // Este trecho está mockado. Nome COBOL: MOVE WQ01-ID-VOL-CMIS2-PCKL TO WS01-PCL-DADOS
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await GravarDetalheRelatorioAsync(volume.IdVolumeCmis2?.ToString() ?? volume.IdVolume.ToString());
        linhas++;

        // Imprime nome do acondicionamento
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0970' TO WS01-PCL-POSICAO-X
        var nomeAcondicionamento = idioma == "E" ? volume.NomeAcondicionamentoIngles : volume.NomeAcondicionamentoEspanhol;
        await GravarDetalheRelatorioAsync(nomeAcondicionamento);
        linhas++;

        // Imprime dimensões e pesos
        await GravarDetalheRelatorioAsync(volume.AlturaVolume.ToString());
        await GravarDetalheRelatorioAsync(volume.ComprimentoVolume.ToString());
        await GravarDetalheRelatorioAsync(volume.LarguraVolume.ToString());
        await GravarDetalheRelatorioAsync(FormatarNumero(volume.MetrosCubicos));
        await GravarDetalheRelatorioAsync(FormatarNumero(volume.PesoTotal));
        await GravarDetalheRelatorioAsync(FormatarNumero(volume.PesoTara));
        await GravarDetalheRelatorioAsync(FormatarNumero(volume.PesoLiquido));
        linhas += 7;

        return linhas;
    }

    private async Task<int> ImprimirTotaisAsync(TotaisPackList totais, MontadorPckListResInput input)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '(s0p12.00h0s3b4099T' TO WS01-LINHA-IMPR-AUX
        await GravarDetalheRelatorioAsync("(s0p12.00h0s3b4099T");
        linhas++;

        // Imprime título do total
        var tituloTotal = input.IdIdioma == "E"
            ? "TOTAL PACKING LIST       ------------------------>"
            : "TOTAL LISTA DE EMBARQUE  ------------------------>";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '1135' TO WS01-PCL-POSICAO-X
        // Este trecho está mockado. Nome COBOL: MOVE '3935' TO WS01-PCL-POSICAO-Y
        await GravarDetalheRelatorioAsync(tituloTotal);
        linhas++;

        // Imprime totais
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.MetrosCubicos));
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.PesoTotal));
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.PesoTara));
        await GravarDetalheRelatorioAsync(FormatarNumero(totais.PesoLiquido));
        linhas += 4;

        return linhas;
    }

    private async Task<int> ImprimirTiposAcondicionamentoAsync(List<TipoAcondicionamentoCount> tipos, string idioma)
    {
        var linhas = 0;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '0000' TO WS01-PCL-POSICAO-X
        // Este trecho está mockado. Nome COBOL: MOVE '4078' TO WS01-PCL-POSICAO-Y
        var posicaoX = "0000";
        var posicaoY = 4078;

        foreach (var tipo in tipos)
        {
            // Controle de posição
            if (posicaoX == "5400")
            {
                posicaoY += 142;
                posicaoX = "0000";
            }

            if (posicaoX == "0000")
            {
                posicaoX = "0200";
            }
            else
            {
                posicaoX = (int.Parse(posicaoX) + 1300).ToString();
            }

            var nomeTipo = idioma == "E" ? tipo.NomeIngles : tipo.NomeEspanhol;

            //mock
            // Este trecho está mockado. Nome COBOL: MOVE WQ01-NM-ACND-TOT-ING TO WS01-PCL-DADOS
            // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
            await GravarDetalheRelatorioAsync(nomeTipo);
            linhas++;
        }

        return linhas;
    }

    private string FormatarNumero(string numero)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-MTS-CUB-VOL CONVERTING '.' TO '!'
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-MTS-CUB-VOL CONVERTING ',' TO '.'
        // Este trecho está mockado. Nome COBOL: INSPECT WQ01-MTS-CUB-VOL CONVERTING '!' TO ','
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

public class VolumePackList
{
    public int IdVolume { get; set; }
    public int? IdVolumeCmis2 { get; set; }
    public string NomeAcondicionamentoIngles { get; set; } = string.Empty;
    public string NomeAcondicionamentoEspanhol { get; set; } = string.Empty;
    public decimal AlturaVolume { get; set; }
    public decimal ComprimentoVolume { get; set; }
    public decimal LarguraVolume { get; set; }
    public string MetrosCubicos { get; set; } = string.Empty;
    public string PesoTotal { get; set; } = string.Empty;
    public string PesoTara { get; set; } = string.Empty;
    public string PesoLiquido { get; set; } = string.Empty;
}

public class TotaisPackList
{
    public string MetrosCubicos { get; set; } = string.Empty;
    public string PesoTotal { get; set; } = string.Empty;
    public string PesoTara { get; set; } = string.Empty;
    public string PesoLiquido { get; set; } = string.Empty;
}

public class TipoAcondicionamentoCount
{
    public string NomeIngles { get; set; } = string.Empty;
    public string NomeEspanhol { get; set; } = string.Empty;
}
