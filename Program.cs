using System;
using System.Threading.Tasks;
using SistemasOperacionais.Maquina;
using SistemasOperacionais.Modelos;
using SistemasOperacionais.Escalonadores;

class Program
{
    static async Task Main(string[] args)
    {
        var memoria = new Memoria(maxSize: 512); // 512MB
        var escalonador = new RoundRobinEscalonador();
        var cpu = new CPU(escalonador, memoria);

        // Criar alguns processos de exemplo
        var p1 = new Processo("EditorTexto", prioridade: 1, memoriaAlocada: 100, tempoExecucao: 500);
        var p2 = new Processo("Compilador", prioridade: 2, memoriaAlocada: 200, tempoExecucao: 800);
        var p3 = new Processo("PlayerMusica", prioridade: 1, memoriaAlocada: 50, tempoExecucao: 300);


        // Alocar memória (simulação)
        Console.WriteLine("Tentando alocar processos na memória...");
        foreach (var p in new[] { p1, p2, p3 })
        {
            if (memoria.AlocarMemoria(p))
                Console.WriteLine($"Alocado: {p}");
            else
                Console.WriteLine($"Falha ao alocar: {p}");
        }

        memoria.MostrarEstadoMemoria();

        // Rodar tudo
        await cpu.RodarTudoAsync();

        memoria.MostrarEstadoMemoria();
    }
}
