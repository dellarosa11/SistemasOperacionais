using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemasOperacionais.Maquina
{
    public class EstatisticasSO
    {
        public int TrocasDeContexto { get; set; } = 0;
        public int CiclosCPUOciosa { get; set; } = 0;
        public int CiclosTotais { get; set; } = 0;
        public int TempoSobrecargaEscalonamento { get; set; } = 0; // Simulação de tempo de sobrecarga

        // Métricas de Memória
        public int FaltasDePagina { get; set; } = 0;
        public int SwapIns { get; set; } = 0;
        public int SwapOuts { get; set; } = 0;

        // Métricas de IO
        public Dictionary<string, int> UtilizacaoDispositivos { get; } = new Dictionary<string, int>();

        public void RegistrarTrocaDeContexto(int sobrecarga)
        {
            TrocasDeContexto++;
            TempoSobrecargaEscalonamento += sobrecarga;
        }

        public void RegistrarCiclo()
        {
            CiclosTotais++;
        }

        public void RegistrarCPUOciosa()
        {
            CiclosCPUOciosa++;
        }

        public void RegistrarUsoDispositivo(string nomeDispositivo)
        {
            if (UtilizacaoDispositivos.ContainsKey(nomeDispositivo))
            {
                UtilizacaoDispositivos[nomeDispositivo]++;
            }
            else
            {
                UtilizacaoDispositivos.Add(nomeDispositivo, 1);
            }
        }

        public string GerarRelatorio()
        {
            StringBuilder relatorio = new StringBuilder();
            relatorio.AppendLine("--- Relatório de Consumo de Recursos e Métricas ---");
            relatorio.AppendLine($"Ciclos Totais de Simulação: {CiclosTotais}");
            relatorio.AppendLine($"Trocas de Contexto: {TrocasDeContexto}");
            relatorio.AppendLine($"Sobrecarga Total de Escalonamento (Simulada): {TempoSobrecargaEscalonamento}ms");
            
            double utilizacaoCPU = CiclosTotais > 0 ? (double)(CiclosTotais - CiclosCPUOciosa) / CiclosTotais * 100 : 0;
            relatorio.AppendLine($"Utilização de CPU: {utilizacaoCPU:F2}%");
            relatorio.AppendLine($"Ciclos de CPU Ociosa: {CiclosCPUOciosa}");

            relatorio.AppendLine("\nMétricas de Memória:");
            relatorio.AppendLine($"Faltas de Página: {FaltasDePagina}");
            relatorio.AppendLine($"Swap-Ins: {SwapIns}");
            relatorio.AppendLine($"Swap-Outs: {SwapOuts}");

            relatorio.AppendLine("\nMétricas de E/S (Utilização por Ciclo):");
            if (UtilizacaoDispositivos.Any())
            {
                foreach (var kvp in UtilizacaoDispositivos)
                {
                    double utilizacaoIO = CiclosTotais > 0 ? (double)kvp.Value / CiclosTotais * 100 : 0;
                    relatorio.AppendLine($"- {kvp.Key}: {utilizacaoIO:F2}% ({kvp.Value} ciclos)");
                }
            }
            else
            {
                relatorio.AppendLine("- Nenhum dispositivo de E/S utilizado.");
            }

            return relatorio.ToString();
        }
    }
}
