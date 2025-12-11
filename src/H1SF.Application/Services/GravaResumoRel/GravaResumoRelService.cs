using H1SF.Application.DTOs.Relatorio;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services;

/// <summary>
/// 550-00-GRAVA-RESUMO-REL
/// Autor: A.C.ANDREATTA
/// </summary>
public class GravaResumoRel550Service : IGravaResumoRel
{
    private readonly ApplicationDbContext _context;

    public GravaResumoRel550Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GravaResumoRelOutput> ExecutarAsync(GravaResumoRelInput input)
    {
        var resultado = new GravaResumoRelOutput
        {
            Sucesso = false,
            ExecutouBypass = false,
            ChaveComandoPjlFinal = input.ChaveComandoPjl
        };

        // IF ((ST0001-CD-REGR-FTRM EQUAL 'M' OR 'K' OR CB0004-CD-T-PRD EQUAL 'C')
        //     AND CB0001-RRE-ID-PRCP-PTD-LIT EQUAL 'PROTOCOL') OR
        //    (ST0001-CD-REGR-FTRM EQUAL 'N' AND CB0001-RRE-ID-REL EQUAL 'DANFE DE PECAS')
        //    GO TO 550-10-RESET-CHV
        bool condicaoBypass1 = ((input.CodigoRegraFaturamento == "M" ||
                                  input.CodigoRegraFaturamento == "K" ||
                                  input.CodigoTipoProduto == "C") &&
                                 input.IdPrincipalProtocoloLiteral == "PROTOCOL");

        bool condicaoBypass2 = (input.CodigoRegraFaturamento == "N" &&
                                input.IdRelatorio == "DANFE DE PECAS");

        if (condicaoBypass1 || condicaoBypass2)
        {
            // GO TO 550-10-RESET-CHV
            resultado.ExecutouBypass = true;
            resultado.ChaveComandoPjlFinal = "N";
            resultado.Sucesso = true;
            return resultado;
        }

        // Determinar código sequência PJL
        string codigoSequenciaPjl = "03"; // default

        if (input.ChaveComandoPjl == "N")
        {
            //mock
            // Este trecho está mockado. Nome COBOL: IF CB0001-RRE-ID-PRCP-PTD-LIT EQUAL 'PROTOCOL' MOVE '01'
            if (input.IdPrincipalProtocoloLiteral == "PROTOCOL")
            {
                codigoSequenciaPjl = "01";
            }
            else if (input.IdPrincipalProtocoloLiteral == "INSTRUCA")
            {
                codigoSequenciaPjl = "02";
            }
            else if (input.IdPrincipalProtocoloLiteral == "PACKLIST")
            {
                codigoSequenciaPjl = "05";
            }
            else if (input.IdPrincipalProtocoloLiteral == "LISTAEMB")
            {
                codigoSequenciaPjl = "06";
            }
        }
        else
        {
            codigoSequenciaPjl = input.ChaveComandoPjl;
        }

        // EXEC SQL INSERT INTO H1CB.RRE_RESREL
        var linhasAfetadas = await _context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO H1CB.RRE_RESREL
                (RRE_CD_STM,
                 RRE_DTC_GRC,
                 RRE_ID_PRCP,
                 RRE_CD_SQN_DCT,
                 RRE_CD_SQN_PJL,
                 RRE_ID_AUX_IMPS_1,
                 RRE_ID_AUX_IMPS_2,
                 RRE_ID_AUX_IMPS_3,
                 RRE_ID_AUX_IMPS_4,
                 RRE_ID_AUX_IMPS_5,
                 RRE_ID_REL,
                 RRE_ID_IMPR)
            VALUES
                ({0},
                 TO_DATE({1}, 'YYYYMMDDHH24MISS'),
                 {2},
                 {3},
                 {4},
                 {5},
                 {6},
                 {7},
                 {8},
                 {9},
                 {10},
                 {11})",
            input.CodigoSistema,
            input.DataHoraGeracao,
            input.IdPrincipal,
            input.CodigoSequenciaDocumento,
            codigoSequenciaPjl,
            input.IdAuxiliarImpressao1,
            input.IdAuxiliarImpressao2,
            input.IdAuxiliarImpressao3,
            input.IdAuxiliarImpressao4,
            input.IdAuxiliarImpressao5,
            input.IdRelatorio,
            input.IdImpressora);

        // 550-10-RESET-CHV
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'N' TO WS31-CHV-COMANDO-PJL
        resultado.ChaveComandoPjlFinal = "N";

        resultado.Sucesso = linhasAfetadas > 0;
        resultado.LinhasAfetadas = linhasAfetadas;

        return resultado;
    }
}
