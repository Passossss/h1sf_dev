using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.Interface_s_de_item
{
    public class InterfaceItemService : IInterfaceItemService
    {
        // Variáveis de trabalho (equivalente às variáveis WS do COBOL)
        private string _ws35AuxTs = string.Empty;
        private decimal _ws01QPecaItmAux = 0;
        private decimal _ws01QTtlItemFat = 0;
        private decimal _ws01RateUsItem = 0;
        private decimal _ws01VTtlFrt = 0;
        private decimal _ws01VTtlSgr = 0;
        private decimal _ws01VTtlOda = 0;
        private decimal _ws01VTtlAredCes9 = 0;
        private decimal _ws01VTtlAi = 0;
        private decimal _ws01TtlFeesCes = 0;
        private decimal _ws01VTtlFrt9 = 0;
        private decimal _ws01VTtlSgr9 = 0;
        private decimal _ws01VTtlOda9 = 0;
        private decimal _ws01RateUsItemR = 0;
        private string _ws01IdNumCntS = string.Empty;
        private string _ws01IdNumTRegS = string.Empty;

        // Variáveis de consulta SQL (equivalente às variáveis WQ do COBOL)
        private decimal _wq01PesoLqdTtlAux = 0;
        private decimal _wq01RateUsItem = 0;
        private decimal _wq01ItdPrecUsApiUntReb = 0;
        private decimal _wq01ItdPrecUsApiTtlReb = 0;
        private decimal _wq01ItdPrecUsDnCoreTtl = 0;
        private decimal _wq01ItdPrecUsIcCoreTtl = 0;
        private string _wq01SysdateS = string.Empty;
        private decimal _wq01TtlFeesCes = 0;
        private string _wq02CdMercDst = string.Empty;
        private string _wq02DtcSelFtrm = string.Empty;
        private string _wq02LgonFunc = string.Empty;

        // Registros COBOL convertidos para classes C#
        private CC0002 _cc0002 = new CC0002();
        private CC0001 _cc0001 = new CC0001();
        private SF0002 _sf0002 = new SF0002();
        private SF0001 _sf0001 = new SF0001();
        private ST0001 _st0001 = new ST0001();
        private ST0005 _st0005 = new ST0005();
        private SE0001 _se0001 = new SE0001();
        private SE90043 _se90043 = new SE90043();
        private SE90243 _se90243 = new SE90243();

        // Serviços de dependência
        private readonly IOracleDatabaseService _databaseService;
        private readonly IInterfaceDeaService _interfaceDeaService;
        private readonly ILogger<InterfaceItemService> _logger;

        public InterfaceItemService(
            IOracleDatabaseService databaseService,
            IInterfaceDeaService interfaceDeaService,
            ILogger<InterfaceItemService> logger)
        {
            _databaseService = databaseService;
            _interfaceDeaService = interfaceDeaService;
            _logger = logger;
        }

        public async Task ProcessarInterfaceItemAsync()
        {
            _logger.LogInformation("Iniciando processamento da interface de item");

            try
            {
                // 880-00-INTERFACE-S-DE-ITEM SECTION
                _ws35AuxTs = "880-00-INTERFACE-S-DE-ITEM";

                // Processar quantidade
                await ProcessarQuantidadeAsync();

                // Consultar peso líquido
                await ConsultarPesoLiquidoAsync();

                // Identificar interface
                await IdentificarInterfaceAsync();

                // Carregar campos
                await CarregarCamposAsync();

                // Gravar interface DEA
                await _interfaceDeaService.GravarInterfaceDeaAsync(_se0001);

                _logger.LogInformation("Processamento da interface de item concluído");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no processamento da interface de item");
                throw;
            }
        }

        private async Task ProcessarQuantidadeAsync()
        {
            // MOVE CC0002-QTD-DEA TO WS01-Q-PECA-ITM-AUX
            // ADD WS01-Q-PECA-ITM-AUX TO WS01-Q-TTL-ITEM-FAT
            //_ws01QPecaItmAux = _cc0002.QtdDea;
            _ws01QTtlItemFat += _ws01QPecaItmAux;
        }

        private async Task ConsultarPesoLiquidoAsync()
        {
            try
            {
                // EXEC SQL SELECT :CC0002-QTD-DEA * NVL(...)/1000
                var sql = @"
                SELECT :qtdDea * 
                       NVL(DECODE(ITD_PESO_LQD_PECA, 0,
                                   ITD_PESO_BRT_PECA,
                                   ITD_PESO_LQD_PECA), 0) / 1000
                FROM H1SF.ITD_ITMFATURADO
                WHERE ITD_CD_MERC_DST = :cdMercDst
                  AND ITD_DTC_SEL_FTRM = :dtcSelFtrm
                  AND ITD_LGON_FUNC = :lgonFunc
                  AND ITD_ID_PECA = :idPeca
                  AND ROWNUM = 1";

                var parameters = new
                {
                    qtdDea = _cc0002.QtdDea,
                    cdMercDst = _sf0001.PtdCdMercDst,
                    dtcSelFtrm = _sf0001.PtdDtcSelFtrm,
                    lgonFunc = _sf0001.PtdLgonFunc,
                    idPeca = _sf0002.ItdIdPecaLog
                };

                _wq01PesoLqdTtlAux = await _databaseService.QuerySingleAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar peso líquido");
                _wq01PesoLqdTtlAux = 0;
            }
        }

        private async Task IdentificarInterfaceAsync()
        {
            // MOVE 'CBL' TO SE0001-IDE-NM-EMP
            _se0001.IdeNmEmp = "CBL";

            // IF ST0001-CD-REGR-FTRM EQUAL 'I'
            if (_st0001.CdRegrFtrm == "I")
            {
                _se0001.IdeIdTItf = "FATURA_S57";
                _se0001.IdeIdStmOrgm = "BPISPRA";
            }
            // ELSE IF ST0001-CD-REGR-FTRM EQUAL 'N'
            else if (_st0001.CdRegrFtrm == "N")
            {
                _se0001.IdeIdTItf = "FATURA_S7X";
                _se0001.IdeIdStmOrgm = "BPISREB";
            }
            else
            {
                _se0001.IdeIdTItf = "FATURA_S58";
                _se0001.IdeIdStmOrgm = "BPIS";
            }

            // Obter data do sistema
            _wq01SysdateS = DateTime.Now.ToString("yyyyMMdd");

            _se0001.IdeDtcGrc = _wq01SysdateS;
            _se0001.IdeDtcItf = _wq01SysdateS;
            _se0001.IdeNumCnt = _ws01IdNumCntS;
            _se0001.IdeNumTReg = _ws01IdNumTRegS;
            _se0001.IdeNumSqn = 0; // ZEROS
            _se0001.IdeIcPrc = "*";
        }

        private async Task CarregarCamposAsync()
        {
            // INSPECT CC0002-QTD-DEA CONVERTING SPACES TO ZEROS
            _cc0002.QtdDea = _cc0002.QtdDea.Replace(' ', '0');

            // Executar consulta de taxas e preços
            await ConsultarTaxasPrecosAsync();

            // Processar INSPECT nas variáveis WQ
            ProcessarConversoesEspacosParaZeros();

            // MOVE WQ01-RATE-US-ITEM TO WS01-RATE-US-ITEM
            _ws01RateUsItem = _wq01RateUsItem;

            // Processar preços baseado no código de regra
            await ProcessarPrecosAsync();

            // Preencher SE90043
            PreencherSe90043();

            // Processar valores de frete, seguro e outras despesas
            await ProcessarValoresAdicionaisAsync();

            // Processar valores AI (Additional Information)
            await ProcessarValoresAiAsync();

            // Definir mensagem de texto baseada no código de regra
            DefinirMensagemTexto();
        }

        private async Task ConsultarTaxasPrecosAsync()
        {
            try
            {
                var sql = @"
                SELECT ITD_RATE_US,
                       ITD_PREC_US_API_UNT - ITD_V_FEE4_DISC_CES,
                       (ITD_PREC_US_API_UNT - ITD_V_FEE4_DISC_CES) * ITD_Q_PECA_FTRD,
                       ITD_PREC_US_DN_CORE * ITD_Q_PECA_FTRD,
                       ITD_PREC_US_IC_CORE * ITD_Q_PECA_FTRD
                FROM H1SF.ITD_ITMFATURADO
                WHERE ITD_CD_MERC_DST = :cdMercDst
                  AND ITD_DTC_SEL_FTRM = :dtcSelFtrm
                  AND ITD_LGON_FUNC = :lgonFunc
                  AND ITD_FTR_EXP = :ftrExp
                  AND ITD_ID_ETIQ_ACND = :idEtiqAcnd
                  AND ITD_ID_NF = :idNf
                  AND ITD_CD_SEQ_ITM = :cdSeqItm
                  AND ROWNUM = 1";

                var parameters = new
                {
                    cdMercDst = _wq02CdMercDst,
                    dtcSelFtrm = _wq02DtcSelFtrm,
                    lgonFunc = _wq02LgonFunc,
                    ftrExp = _sf0002.ItdFtrExp,
                    idEtiqAcnd = _sf0002.ItdIdEtiqAcndLog,
                    idNf = _cc0001.Numero,
                    cdSeqItm = _cc0002.IdfNum
                };

                var result = await _databaseService.QuerySingleAsync<(decimal, decimal, decimal, decimal, decimal)>(sql, parameters);

                _wq01RateUsItem = result.Item1;
                _wq01ItdPrecUsApiUntReb = result.Item2;
                _wq01ItdPrecUsApiTtlReb = result.Item3;
                _wq01ItdPrecUsDnCoreTtl = result.Item4;
                _wq01ItdPrecUsIcCoreTtl = result.Item5;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar taxas e preços");
                // Inicializar com zeros em caso de erro
                _wq01RateUsItem = 0;
                _wq01ItdPrecUsApiUntReb = 0;
                _wq01ItdPrecUsApiTtlReb = 0;
                _wq01ItdPrecUsDnCoreTtl = 0;
                _wq01ItdPrecUsIcCoreTtl = 0;
            }
        }

        private void ProcessarConversoesEspacosParaZeros()
        {
            // INSPECT das variáveis WQ convertendo espaços para zeros
            // Em C#, tratamos como conversão de string para decimal
            // Na prática, esses valores já seriam decimais no C#
            // Esta função é mantida para fidelidade com o COBOL
        }

        private async Task ProcessarPrecosAsync()
        {
            if (_st0001.CdRegrFtrm == "N")
            {
                // Para código 'N'
                _cc0002.PrecoUnitDeaUsX = _wq01ItdPrecUsApiUntReb;
                _cc0002.PrecoTotDeaUsX = _wq01ItdPrecUsApiTtlReb;
            }
            else
            {
                // Para outros códigos
                // COMPUTE com ROUNDED
                _cc0002.PrecoTotalDeaUs = Math.Round(
                    _cc0002.PrecoTotDeaN / (_ws01RateUsItemR / 10000),
                    2, MidpointRounding.AwayFromZero);

                _cc0002.PrecoUnitarioDeaUs = Math.Round(
                    _cc0002.PrecoUnitarioDeaN / (_ws01RateUsItemR / 10000),
                    2, MidpointRounding.AwayFromZero);
            }
        }

        private void PreencherSe90043()
        {
            _se90043.IdFtrS57 = _sf0002.ItdFtrExp.Replace(' ', '0');
            _se90043.IdPeca = _sf0002.ItdIdPecaLog;
            _se90043.IdNbm = _cc0002.NbmCodigoDea;
            _se90043.QtPeca = _cc0002.QtdDea;
            _se90043.VItem = _cc0002.PrecoUnitDeaUsX;
            _se90043.VTtlItem = _cc0002.PrecoTotDeaUsX;
            _se90043.UndPesoR = _wq01PesoLqdTtlAux;
            _se90043.IdVol = _sf0002.ItdIdVolLog;
            _se90043.IdEtiqRec = _sf0002.ItdIdEtiqRecLog;
            _se90043.IdEtiqAcnd = _sf0002.ItdIdEtiqAcndLog;
            _se90043.IdItemDe = _sf0002.ItdIdItmDe;
            _se90043.CdPfo = _st0005.CdPfo;

            if (_st0001.CdRegrFtrm == "N")
            {
                _se90043.VTtlCasc = _wq01ItdPrecUsDnCoreTtl;
            }
            else
            {
                _se90043.VTtlCasc = _wq01ItdPrecUsIcCoreTtl;
            }
        }

        private async Task ProcessarValoresAdicionaisAsync()
        {
            // Frete
            _ws01VTtlFrt9 = _sf0002.ItdVUsRtFrt;
            _ws01VTtlFrt = _ws01VTtlFrt9;
            _se90043.VTtlFrt = _ws01VTtlFrt;

            // Seguro
            _ws01VTtlSgr9 = _sf0002.ItdVUsRtSgr;
            _ws01VTtlSgr = _ws01VTtlSgr9;
            _se90043.VTtlSgr = _ws01VTtlSgr;

            // Outras despesas
            _ws01VTtlOda9 = _sf0002.ItdVUsRtOda;

            if (_st0001.CdRegrFtrm != "N")
            {
                _ws01VTtlAredCes9 = _sf0002.ItdVUsAredCesOda;
                _ws01VTtlOda += _ws01VTtlAredCes9;
            }

            _se90043.VTtlOda = _ws01VTtlOda;
        }

        private async Task ProcessarValoresAiAsync()
        {
            _ws01VTtlAi = _sf0002.ItdVAiChrgApi;

            if (_st0001.CdRegrFtrm == "N")
            {
                await ConsultarTotalFeesCesAsync();

                _ws01TtlFeesCes = _wq01TtlFeesCes;
                _ws01VTtlAi += _ws01TtlFeesCes;
            }

            _se90043.VTtlAi = _ws01VTtlAi;
            _se90043.VTtlEmgChrg = _sf0002.ItdVEmgChrgApi;
            _se90043.VTtlDisc = _sf0002.ItdVMktDiscApi;
        }

        private async Task ConsultarTotalFeesCesAsync()
        {
            try
            {
                var sql = @"
                SELECT SUM((ITD_Q_PECA_FTRD * ITD_V_FEE1_CES) +
                           (ITD_Q_PECA_FTRD * ITD_V_FEE2_CES) +
                           (ITD_Q_PECA_FTRD * ITD_V_FEE3_CES)) +
                       MAX(ITD_V_US_ARED_CES_ODA)
                FROM H1SF.ITD_ITMFATURADO
                WHERE ITD_CD_MERC_DST = :cdMercDst
                  AND ITD_DTC_SEL_FTRM = :dtcSelFtrm
                  AND ITD_LGON_FUNC = :lgonFunc";

                var parameters = new
                {
                    cdMercDst = _sf0001.PtdCdMercDst,
                    dtcSelFtrm = _sf0001.PtdDtcSelFtrm,
                    lgonFunc = _sf0001.PtdLgonFunc
                };

                _wq01TtlFeesCes = await _databaseService.QuerySingleAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar total fees CES");
                _wq01TtlFeesCes = 0;
            }
        }

        private void DefinirMensagemTexto()
        {
            if (_st0001.CdRegrFtrm == "I")
            {
                _se0001.IdeTxtMsg = _se90043.TxtMsg;
            }
            else
            {
                _se90243.TxtMsg = _se90043.TxtMsg;
                _se0001.IdeTxtMsg = _se90243.TxtMsg;
            }
        }
    }

    // Classes de DTO (Data Transfer Objects)
    public class CC0002
    {
        public string QtdDea { get; set; } = string.Empty;
        public string NbmCodigoDea { get; set; } = string.Empty;
        public decimal PrecoUnitDeaUsX { get; set; }
        public decimal PrecoTotDeaUsX { get; set; }
        public decimal PrecoTotalDeaUs { get; set; }
        public decimal PrecoUnitarioDeaUs { get; set; }
        public decimal PrecoTotDeaN { get; set; }
        public decimal PrecoUnitarioDeaN { get; set; }
        public string IdfNum { get; set; } = string.Empty;
    }

    public class CC0001
    {
        public string Numero { get; set; } = string.Empty;
    }

    public class SF0002
    {
        public string ItdFtrExp { get; set; } = string.Empty;
        public string ItdIdPecaLog { get; set; } = string.Empty;
        public string ItdIdVolLog { get; set; } = string.Empty;
        public string ItdIdEtiqRecLog { get; set; } = string.Empty;
        public string ItdIdEtiqAcndLog { get; set; } = string.Empty;
        public string ItdIdItmDe { get; set; } = string.Empty;
        public decimal ItdVUsRtFrt { get; set; }
        public decimal ItdVUsRtSgr { get; set; }
        public decimal ItdVUsRtOda { get; set; }
        public decimal ItdVUsAredCesOda { get; set; }
        public decimal ItdVAiChrgApi { get; set; }
        public decimal ItdVEmgChrgApi { get; set; }
        public decimal ItdVMktDiscApi { get; set; }
    }

    public class SF0001
    {
        public string PtdCdMercDst { get; set; } = string.Empty;
        public string PtdDtcSelFtrm { get; set; } = string.Empty;
        public string PtdLgonFunc { get; set; } = string.Empty;
    }

    public class ST0001
    {
        public string CdRegrFtrm { get; set; } = string.Empty;
    }

    public class ST0005
    {
        public string CdPfo { get; set; } = string.Empty;
    }

    public class SE0001
    {
        public string IdeNmEmp { get; set; } = string.Empty;
        public string IdeIdTItf { get; set; } = string.Empty;
        public string IdeIdStmOrgm { get; set; } = string.Empty;
        public string IdeDtcGrc { get; set; } = string.Empty;
        public string IdeDtcItf { get; set; } = string.Empty;
        public string IdeNumCnt { get; set; } = string.Empty;
        public string IdeNumTReg { get; set; } = string.Empty;
        public int IdeNumSqn { get; set; }
        public string IdeIcPrc { get; set; } = string.Empty;
        public string IdeTxtMsg { get; set; } = string.Empty;
    }

    public class SE90043
    {
        public string IdFtrS57 { get; set; } = string.Empty;
        public string IdPeca { get; set; } = string.Empty;
        public string IdNbm { get; set; } = string.Empty;
        public string QtPeca { get; set; } = string.Empty;
        public decimal VItem { get; set; }
        public decimal VTtlItem { get; set; }
        public decimal UndPesoR { get; set; }
        public string IdVol { get; set; } = string.Empty;
        public string IdEtiqRec { get; set; } = string.Empty;
        public string IdEtiqAcnd { get; set; } = string.Empty;
        public string IdItemDe { get; set; } = string.Empty;
        public string CdPfo { get; set; } = string.Empty;
        public decimal VTtlCasc { get; set; }
        public decimal VTtlFrt { get; set; }
        public decimal VTtlSgr { get; set; }
        public decimal VTtlOda { get; set; }
        public decimal VTtlAi { get; set; }
        public decimal VTtlEmgChrg { get; set; }
        public decimal VTtlDisc { get; set; }
        public string TxtMsg { get; set; } = string.Empty;
    }

    public class SE90243
    {
        public string TxtMsg { get; set; } = string.Empty;
    }

    // Interface para serviços de dependência
    public interface IOracleDatabaseService
    {
        Task<T> QuerySingleAsync<T>(string sql, object parameters = null);
    }

    public interface IInterfaceDeaService
    {
        Task GravarInterfaceDeaAsync(SE0001 se0001);
    }
}
