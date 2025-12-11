using H1SF.Application.DTOs.Protocolo;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services;

/// <summary>
/// 540-00-ATUALIZA-PROTOCOLO
/// Autor: A.C.ANDREATTA
/// </summary>
public class AtualizadorProtocoloService : IAtualizadorProtocolo
{
    private readonly ApplicationDbContext _context;

    public AtualizadorProtocoloService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AtualizadorProtocoloOutput> ExecutarAsync(AtualizadorProtocoloInput input)
    {
        var resultado = new AtualizadorProtocoloOutput
        {
            Sucesso = false
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PRECO-TOTAL-M-TOT TO WS01-PTD-V-TTL-MRCD-R
        var valorMercadoriaRedefine = input.PrecoTotalMercadoria;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PTD-V-TTL-MRCD TO WQ01-PTD-V-TTL-MRCD
        var valorMercadoria = valorMercadoriaRedefine;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PESO-BRUTO-KG-TOT TO WS01-PTD-PESO-TTL-BRT-R
        var pesoBrutoRedefine = input.PesoBrutoKgTotal;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PTD-PESO-TTL-BRT TO WQ01-PTD-PESO-TTL-BRT
        var pesoBruto = pesoBrutoRedefine;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PESO-LIQUIDO-KG-TOT TO WS01-PTD-PESO-TTL-LQD-R
        var pesoLiquidoRedefine = input.PesoLiquidoKgTotal;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PTD-PESO-TTL-LQD TO WQ01-PTD-PESO-TTL-LQD
        var pesoLiquido = pesoLiquidoRedefine;

        // EXEC SQL UPDATE H1SF.PTD_PROTODSP
        // SET PTD_IC_DSP_IMPS = 'S',
        //     PTD_PESO_TTL_LQD = :WQ01-PTD-PESO-TTL-LQD,
        //     PTD_PESO_TTL_BRT = :WQ01-PTD-PESO-TTL-BRT,
        //     PTD_V_TTL_MRCD = :WQ01-PTD-V-TTL-MRCD
        // WHERE PTD_ID_PTC_DSP = :SF0001-PTD-ID-PTC-DSP
        var linhasAfetadas = await _context.Database.ExecuteSqlRawAsync(@"
            UPDATE H1SF.PTD_PROTODSP
            SET PTD_IC_DSP_IMPS = 'S',
                PTD_PESO_TTL_LQD = {0},
                PTD_PESO_TTL_BRT = {1},
                PTD_V_TTL_MRCD = {2}
            WHERE PTD_ID_PTC_DSP = {3}",
            pesoLiquido,
            pesoBruto,
            valorMercadoria,
            input.IdProtocolo);

        resultado.Sucesso = linhasAfetadas > 0;
        resultado.LinhasAfetadas = linhasAfetadas;

        return resultado;
    }
}
