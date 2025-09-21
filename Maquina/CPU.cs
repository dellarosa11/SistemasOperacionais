using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Maquina
{
    public class CPU
    {
        private readonly IEscalonador _escalonador;
        private readonly Memoria _memoria;
        private readonly Queue<ModeloProcesso> _filaDeNovos = new Queue<ModeloProcesso>();

        // Ajuste aqui para controlar a velocidade da simulação
        private const int FatorLentidao = 5;        // multiplica o tempo de execução
        private const int PausaEntreCiclosMs = 2000; // pausa entre ciclos para leitura

        public CPU(IEscalonador escalonador, Memoria memoria)
        {
            _escalonador = escalonador ?? throw new ArgumentNullException(nameof(escalonador));
            _memoria = memoria ?? throw new ArgumentNullException(nameof(memoria));
        }

        /// Adiciona novos processos na fila de espera (ainda fora da memória).
        public void AdicionarProcessos(IEnumerable<ModeloProcesso> processos)
        {
            if (processos == null) return;
            foreach (var p in processos)
                _filaDeNovos.Enqueue(p);
        }

        /// Tenta carregar automaticamente novos processos na memória (First Fit).
        /// Mantém a ordem da fila: se o primeiro não couber, para (poderia expandir para rotação).
        private void CarregarNovos()
        {
            int tentativas = _filaDeNovos.Count;
            for (int i = 0; i < tentativas; i++)
            {
                var processo = _filaDeNovos.Peek();
                if (_memoria.AlocarMemoria(processo))
                {
                    Console.WriteLine($"[LOAD] {processo} carregado na memória.");
                    _filaDeNovos.Dequeue();
                }
                else
                {
                    // não coube agora → tenta no próximo ciclo
                    break;
                }
            }
        }

        /// Roda uma iteração de execução.
        public async Task RodarAsync(int ciclo)
        {
            Console.WriteLine($"\n=== CICLO {ciclo} ===");

            // tenta carregar novos processos
            CarregarNovos();

            // pega os prontos (garante que 'prontos' existe)
            var prontos = _memoria.ProcessosNaMemoria
                .Where(p => p.Estado == EstadoProcesso.Pronto)
                .ToList();

            var proximo = _escalonador.DecidirProximoProcesso(prontos);
            if (proximo == null)
            {
                Console.WriteLine("[INFO] Nenhum processo pronto.");
                // pausa curta para não travar o loop (e para leitura)
                await Task.Delay(PausaEntreCiclosMs);
                return;
            }

            Console.WriteLine($"[EXEC] {proximo} rodando por {proximo.TempoExecucao}ms");
            proximo.Executar();

            // calcula delay com proteção contra overflow
            long delayLong = (long)proximo.TempoExecucao * (long)FatorLentidao;
            int delayToUse = delayLong > int.MaxValue ? int.MaxValue : (int)delayLong;

            await Task.Delay(Math.Max(0, delayToUse));

            proximo.Finalizar();
            Console.WriteLine($"[FIM]  {proximo} finalizado");

            _memoria.LiberarMemoria(proximo);
            _memoria.MostrarEstadoMemoria();

            Console.WriteLine($"=== FIM CICLO {ciclo} ===");

            // pausa extra entre ciclos para leitura
            await Task.Delay(PausaEntreCiclosMs);
        }

        /// Executa várias iterações até não sobrar processos na memória nem na fila.
        public async Task RodarTudoAsync()
        {
            int ciclo = 1;
            while (_memoria.ProcessosNaMemoria.Any(p => p.Estado == EstadoProcesso.Pronto) || _filaDeNovos.Any())
            {
                await RodarAsync(ciclo++);
            }

            Console.WriteLine("CPU: Todos os processos foram finalizados.");
        }
    }
}
