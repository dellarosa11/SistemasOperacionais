using SistemasOperacionais.Escalonadores;
using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Modelos;
using System.Collections.Generic;

namespace SistemasOperacionais.Maquina
{
    public class SistemaOperacional
    {
        private static SistemaOperacional? _instancia;
        private static readonly object _lock = new object();

        public IEscalonador EscalonadorCPU { get; private set; }
        public GerenciadorMemoria GerenciadorMemoria { get; private set; }
        public GerenciadorIO GerenciadorIO { get; private set; }
        public GerenciadorArquivos GerenciadorArquivos { get; private set; }
        public EstatisticasSO Estatisticas { get; private set; }
        public List<Processo> Processos { get; private set; } = new List<Processo>();
        public List<ThreadProcesso> Threads { get; private set; } = new List<ThreadProcesso>();

        // Construtor privado para implementar o Singleton
        private SistemaOperacional(IEscalonador escalonador)
        {
            EscalonadorCPU = escalonador;
            // Inicializa Gerenciadores de Hardware (exemplo: 4MB de página, 10 molduras = 40MB total)
            // Reduzindo o número de molduras para forçar o swapping
            GerenciadorMemoria = new GerenciadorMemoria(tamanhoPaginaMB: 4, numMolduras: 10);
            GerenciadorIO = new GerenciadorIO();
            GerenciadorArquivos = new GerenciadorArquivos();
            Estatisticas = new EstatisticasSO();
        }

        public static SistemaOperacional Instancia(IEscalonador? escalonador = null)
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        if (escalonador == null)
                        {
                            throw new InvalidOperationException("O escalonador deve ser fornecido na primeira chamada.");
                        }
                        _instancia = new SistemaOperacional(escalonador);
                    }
                }
            }
            return _instancia;
        }

        public void Inicializar()
        {
            // Lógica de inicialização do SO, como carregar o primeiro processo (idle)
            Console.WriteLine("Sistema Operacional Inicializado.");
        }

        public void AdicionarProcesso(Processo processo)
        {
            // 1. Tenta alocar memória
            if (GerenciadorMemoria.Alocar(processo))
            {
                // 2. Se alocado, adiciona à lista de processos e ao escalonador
                Processos.Add(processo);
                EscalonadorCPU.Adicionar(processo);
            }
            else
            {
                Console.WriteLine($"[SO] Processo PID {processo.Id} não pode ser iniciado por falta de memória.");
            }
        }

        public void ExecutarCiclo()
        {
            // Simulação de um ciclo de clock
            Estatisticas.RegistrarCiclo();
            Console.WriteLine($"\n--- Novo Ciclo de Execução (Clock: {Estatisticas.CiclosTotais}) ---");

            // 0. Processar IO (simula interrupções de IO)
            GerenciadorIO.ProcessarIO();

            // 1. Escalonar o próximo elemento (Processo ou Thread)
            ModeloProcesso proximo = EscalonadorCPU.Proximo();

            if (proximo != null)
            {
                // 2. Simular Troca de Contexto (se necessário)
                // TODO: Implementar lógica de troca de contexto e contagem de sobrecarga
                Estatisticas.RegistrarTrocaDeContexto(sobrecarga: 1); // Simula 1ms de sobrecarga por troca

                // 3. Executar o elemento
                proximo.Executar();

                // 4. Verificar se o elemento terminou ou foi bloqueado
                if (proximo.Estado == EstadoProcesso.Finalizado)
                {
                    // 4. Se finalizado, libera recursos
                    Console.WriteLine($"Elemento {proximo.Id} Finalizado.");
                    if (proximo is Processo processoFinalizado)
                    {
                        GerenciadorMemoria.Desalocar(processoFinalizado);
                        // Libera arquivos abertos pelo processo
                        foreach (var nomeArquivo in processoFinalizado.ArquivosAbertos.ToList())
                        {
                            GerenciadorArquivos.FecharArquivo(nomeArquivo, processoFinalizado);
                        }
                    }
                }
                else if (proximo.Estado == EstadoProcesso.Bloqueado)
                {
                    // O elemento já foi bloqueado pelo GerenciadorIO
                    Console.WriteLine($"Elemento {proximo.Id} Bloqueado (aguardando IO).");
                }
                else if (proximo.Estado == EstadoProcesso.Executando)
                {
                    // Se for Round Robin, pode voltar para a fila de prontos
                    // TODO: Implementar lógica de preempção/fim de quantum
                    proximo.Desbloquear(); // Desbloquear move para Pronto
                    EscalonadorCPU.Adicionar(proximo);
                }
            }
            else
            {
                Estatisticas.RegistrarCPUOciosa();
                Console.WriteLine("CPU Ociosa (Fila de Prontos Vazia).");
            }
        }

        // TODO: Implementar métodos para Gerenciamento de Memória, IO e Arquivos
    }
}
