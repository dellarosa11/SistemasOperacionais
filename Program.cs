using SistemasOperacionais.Maquina;
using SistemasOperacionais.Modelos;
using SistemasOperacionais.Escalonadores;

class Program
{
    static async Task Main(string[] args)
    {
        var memoria = new Memoria(maxSize: 64);
        var escalonador = new RoundRobinEscalonador();
        var cpu = new CPU(escalonador, memoria);

        // cria alguns processos
        var processos = new[]
        {
            new Processo("EditorTexto", prioridade: 1, memoriaAlocada: 10, tempoExecucao: 500),
            new Processo("Compilador", prioridade: 2, memoriaAlocada: 20, tempoExecucao: 800),
            new Processo("PlayerMusica", prioridade: 1, memoriaAlocada: 8, tempoExecucao: 300),
            new Processo("Navegador", prioridade: 3, memoriaAlocada: 16, tempoExecucao: 600),
            new Processo("Planilha", prioridade: 1, memoriaAlocada: 12, tempoExecucao: 400)
        };

        Console.WriteLine("Carregando processos na FILA (não diretamente na memória)...\n");
        cpu.AdicionarProcessos(processos);

        // CPU cuida de tudo automaticamente
        await cpu.RodarTudoAsync();
        
    }
}
