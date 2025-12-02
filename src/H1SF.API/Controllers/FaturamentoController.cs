using H1SF.API.DTOs;
using H1SF.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

[ApiController]
[Route("api/faturamento")]
public class FaturamentoController : ControllerBase
{
    private readonly ProcessadorFaturamentoService _service;
    
    public FaturamentoController(ProcessadorFaturamentoService service)
    {
        _service = service;
    }
    
    [HttpPost("processar")]
    public async Task<IActionResult> Processar([FromBody] IniciarFaturamentoRequest request)
    {
        try
        {
            // Constrói string de 24 bytes conforme layout COBOL (PIC X(024))
            var parametros24Bytes = 
                $"{request.CodigoMercadoriaDestino}" +           // byte 0
                $"{request.TimestampSelecao.PadRight(14, ' ')}" + // bytes 1-14
                $"{request.LoginFuncionario.PadRight(8, ' ')}" +  // bytes 15-22
                $"{request.FaseFaturamento}";                     // byte 23
            
            // PERFORM 570-00-RETRIEVE-PARAMETRO
            var resultado = await _service.ProcessarFaturamentoAsync(parametros24Bytes);
            
            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Parâmetros recuperados e validados",
                Parametros = new
                {
                    CodigoMercadoriaDestino = resultado.CodigoMercadoriaDestino,
                    DataHoraSelecao = resultado.DataHoraSelecao.Trim(),
                    LoginFuncionario = resultado.LoginFuncionario.Trim(),
                    FaseFaturamento = resultado.FaseFaturamento
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Sucesso = false, Erro = ex.Message });
        }
    }
}
