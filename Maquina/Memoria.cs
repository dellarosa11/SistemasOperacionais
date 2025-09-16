using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Maquina
{
    public class Memoria
    {
        public int MaxSize { get; private set; } = 1024; // MB
        public int UsedMemory { get; private set; } = 0;

        private readonly List<Processo> ProcessosNaMemoria = new List<Processo>();

        // Construtor
        public Memoria(int maxSize = 1024)
        {
            MaxSize = maxSize;
            Tamanho = Tamanho;
        }

        // Aloca memória para um processo
        public bool AlocarMemoria(Processo processo)
        {
            if (UsedMemory + processo.Tamanho <= MaxSize)
            {
                ProcessosNaMemoria.Add(processo);
                UsedMemory += processo.Tamanho;
                return true;
            }
            return false;
        }

        // Libera memória de um processo
        public void LiberarMemoria(Processo processo)
        {
            if (ProcessosNaMemoria.Contains(processo))
            {
                ProcessosNaMemoria.Remove(processo);
                UsedMemory -= processo.Tamanho;
            }
        }

        // Mostra estado atual da memória
        public void MostrarEstadoMemoria()
        {
            Console.WriteLine($"Memória Total: {MaxSize} MB");
            Console.WriteLine($"Memória Usada: {UsedMemory} MB");
            Console.WriteLine($"Memória Livre: {MaxSize - UsedMemory} MB");

            if (ProcessosNaMemoria.Any())
            {
                Console.WriteLine("\nProcessos na memória:");
                foreach (var processo in ProcessosNaMemoria)
                {
                    Console.WriteLine($"- {processo.Nome} ({processo.Tamanho} MB)");
                }
            }
            else
            {
                Console.WriteLine("\nNenhum processo na memória.");
            }
        }
    }
}
