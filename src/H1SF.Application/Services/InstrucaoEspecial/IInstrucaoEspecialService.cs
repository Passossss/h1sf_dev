using System;

namespace H1SF.Application.Services.InstrucaoEspecial
{
    /// <summary>
    /// Interface para o módulo 960-00-INSTRUCAO-ESPECIAL.
    /// Expõe as entradas principais, resultados e o método de execução.
    /// </summary>
    public interface IInstrucaoEspecial960Service
    {
        // Entradas / contexto
        string SF0001PtdCdTMtz { get; set; }
        int WQ01IdMtzAux { get; set; }
        int WQ01IdCliAux { get; set; }
        int WQ01IdPdd { get; set; }

        // Constante/controle de SQL NOT FOUND (injetável)
        int WS31ChvNtfdSql { get; set; }

        // Campos usados para impressão/posicionamento que podem ser lidos/ajustados pelo chamador
        string WS01Pcl { get; set; }
        string WS01PclReset { get; set; }
        int WS01PclPosicaoY { get; set; }
        int WS01PclResetY { get; set; }

        // Saídas / campos atualizados pela rotina
        string CB0002DreCdSqnPjl { get; set; }

        // Execução da seção principal (corresponde a 960-00)
        void Execute();
    }
}