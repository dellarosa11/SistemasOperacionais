using SistemasOperacionais.Escalonadores;
using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Modelos;
using System;

namespace SistemasOperacionais.Maquina
{
    public static class Boot
    {
        public static SistemaOperacional IniciarSistema()
        {
            Console.WriteLine("--- Iniciando o Sistema Operacional ---");

            // 1. Configuração do Escalonador
            // Exemplo: Round Robin com quantum 20
            IEscalonador escalonador = new RoundRobinEscalonador(20);

            // 2. Inicialização do SO (Singleton)
            SistemaOperacional so = SistemaOperacional.Instancia(escalonador);
            so.Inicializar();

            // 3. Criação de Processos e Threads Iniciais (Workload)
            Console.WriteLine("Carregando Workload Inicial...");
            
            // Processo(nome, prioridade, memoriaAlocada, tempoExecucao)
            // Total de memória disponível: 100 molduras * 4MB/moldura = 400MB
            // Alterando a ordem de criação e tempos de execução para simulação
            var processo1 = new Processo("Navegador", 3, 80, 100); // 20 páginas
            var processo2 = new Processo("Compilador", 1, 150, 200); // 38 páginas
            var processo3 = new Processo("EditorTexto", 2, 30, 50); // 8 páginas
            var processo4 = new Processo("PlayerMusica", 3, 20, 30); // 5 páginas
            // Processo que deve falhar na alocação de memória (98 páginas já alocadas)
            var processo5 = new Processo("Backup", 1, 100, 10); // 25 páginas

            // Interações de E/S:
            // 1. Navegador (P1) faz uma requisição de rede (simulada como IO)
            processo1.ChamarIO("Disco Rígido 1", "Requisição HTTP", 50);
            
            // 2. Compilador (P2) faz uma leitura de arquivo grande
            processo2.ChamarIO("Disco Rígido 1", "Leitura de Código", 150);

            // 3. Editor de Texto (P3) espera por entrada do usuário
            processo3.ChamarIO("Teclado", "Entrada de Usuário", 10);

            // 4. Adiciona elementos ao SO
            // A ordem de adição é a ordem de chegada (FCFS) para a fila de prontos
            so.AdicionarProcesso(processo1);
            so.AdicionarProcesso(processo2);
            so.AdicionarProcesso(processo3);
            so.AdicionarProcesso(processo4);
            so.AdicionarProcesso(processo5); // Este deve falhar na alocação de memória
            
            // Teste de Sistema de Arquivos
            var fs = SistemaOperacional.Instancia().GerenciadorArquivos;
            
            // Criação de arquivos
            fs.CriarArquivo("config.ini", fs.Raiz);
            fs.CriarArquivo("dados.txt", fs.Raiz);

            // Operações de P1 (Navegador)
            processo1.AbrirArquivo("config.ini");
            processo1.EscreverArquivo("config.ini", 10); // 10KB
            processo1.FecharArquivo("config.ini");

            // Operações de P2 (Compilador)
            processo2.AbrirArquivo("dados.txt");
            processo2.EscreverArquivo("dados.txt", 100); // 100KB
            
            // Teste de Memória Virtual: Acessar páginas para forçar Swap-In e Swap-Out
            // P1 (Navegador) tem 20 páginas (0 a 19)
            Console.WriteLine("\n--- Teste de Memória Virtual ---");
            processo1.AcessarMemoria(0); // Falta de página -> Swap-In (Moldura 0)
            processo1.AcessarMemoria(1); // Falta de página -> Swap-In (Moldura 1)
            processo1.AcessarMemoria(2); // Falta de página -> Swap-In (Moldura 2)
            processo1.AcessarMemoria(3); // Falta de página -> Swap-In (Moldura 3)
            processo1.AcessarMemoria(4); // Falta de página -> Swap-In (Moldura 4)
            processo1.AcessarMemoria(5); // Falta de página -> Swap-In (Moldura 5)
            processo1.AcessarMemoria(6); // Falta de página -> Swap-In (Moldura 6)
            processo1.AcessarMemoria(7); // Falta de página -> Swap-In (Moldura 7)
            processo1.AcessarMemoria(8); // Falta de página -> Swap-In (Moldura 8)
            processo1.AcessarMemoria(9); // Falta de página -> Swap-In (Moldura 9) (Molduras 0-9 ocupadas)

            // P2 (Compilador) tem 38 páginas (0 a 37)
            processo2.AcessarMemoria(0); // Falta de página -> Swap-In (Moldura 0 é vítima - FIFO) -> Swap-Out P1:0
            processo2.AcessarMemoria(1); // Falta de página -> Swap-In (Moldura 1 é vítima - FIFO) -> Swap-Out P1:1
            processo2.AcessarMemoria(2); // Falta de página -> Swap-In (Moldura 2 é vítima - FIFO) -> Swap-Out P1:2
            
            // P1 acessa uma página que foi swappada
            processo1.AcessarMemoria(0); // Falta de página -> Swap-In (Moldura 3 é vítima - FIFO) -> Swap-Out P1:3
            
            // Listar diretório
            fs.ListarDiretorio(fs.Raiz);

            // ThreadProcesso(nome, processoPai, prioridade, tempoExecucao)
            var thread1A = new ThreadProcesso("Thread Render", processo1, 3, 20);
            var thread2A = new ThreadProcesso("Thread Garbage", processo1, 1, 10);
            
            // Adiciona threads ao escalonador para simular escalonamento misto
            escalonador.Adicionar(thread1A);
            escalonador.Adicionar(thread2A);

            Console.WriteLine("Inicialização Concluída.");
            return so;
        }
    }
}
