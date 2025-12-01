using SistemasOperacionais.Modelos;
using SistemasOperacionais.Escalonadores;
using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Maquina;

class Program
{
    static void Main(string[] args)
    {
        // 1. Inicializa o Sistema Operacional através da classe Boot
        SistemaOperacional so = Boot.IniciarSistema();

        // Recupera os elementos para exibição (apenas para teste)
        var processo1 = so.Processos.Find(p => p.Nome == "Navegador")!;
        var processo2 = so.Processos.Find(p => p.Nome == "Compilador")!;
        var processo3 = so.Processos.Find(p => p.Nome == "EditorTexto")!;
        var processo4 = so.Processos.Find(p => p.Nome == "PlayerMusica")!;
        var processo5 = so.Processos.Find(p => p.Nome == "Backup"); // Pode ser null se a alocação falhar
        // Nota: Threads não são armazenadas diretamente em so.Threads, mas sim no escalonador.
        // Para fins de teste, vamos apenas simular a execução.

        Console.WriteLine("\nSimulação de Escalonamento de Processos e Threads (Round Robin)");
        Console.WriteLine("----------------------------------------------------------");

        // 2. Simulação de execução por ciclos
        // Aumentando o número de ciclos para observar mais interações de IO
        for (int i = 0; i < 20; i++)
        {
            so.ExecutarCiclo();
        }

        // 3. Exibição do Estado Final
        Console.WriteLine("\nEstado Final dos Elementos:");
        Console.WriteLine(processo1.ExibirElemento());
        Console.WriteLine(processo2.ExibirElemento());
        Console.WriteLine(processo3.ExibirElemento());
        Console.WriteLine(processo4.ExibirElemento());
        if (processo5 != null)
        {
            Console.WriteLine(processo5.ExibirElemento());
        }
        else
        {
            Console.WriteLine("Processo Backup (falhou na alocação de memória) - Não está na lista de processos do SO.");
        }

        // 4. Exibição do Relatório de Métricas
        Console.WriteLine(so.Estatisticas.GerarRelatorio());
    }
}
