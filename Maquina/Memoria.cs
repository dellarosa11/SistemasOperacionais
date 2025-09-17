using System;
using System.Collections.Generic;
using System.Linq;
using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Maquina
{
    public class Memoria
    {
        public int MaxSize { get; private set; } = 1024; // MB
        public int UsedMemory { get; private set; } = 0;

        private readonly List<ModeloProcesso> _processosNaMemoria = new List<ModeloProcesso>();

        public Memoria(int maxSize = 1024)
        {
            if (maxSize <= 0) throw new ArgumentException("maxSize precisa ser > 0");
            MaxSize = maxSize;
        }

        public bool AlocarMemoria(ModeloProcesso processo)
        {
            if (processo == null) throw new ArgumentNullException(nameof(processo));
            if (UsedMemory + processo.MemoriaAlocada <= MaxSize)
            {
                _processosNaMemoria.Add(processo);
                UsedMemory += processo.MemoriaAlocada;
                return true;
            }
            return false;
        }

        public void LiberarMemoria(ModeloProcesso processo)
        {
            if (processo == null) return;
            if (_processosNaMemoria.Remove(processo))
            {
                UsedMemory -= processo.MemoriaAlocada;
                if (UsedMemory < 0) UsedMemory = 0;
            }
        }

        public IReadOnlyList<ModeloProcesso> ProcessosNaMemoria => _processosNaMemoria.AsReadOnly();

        public void MostrarEstadoMemoria()
        {
            Console.WriteLine($"Memória Total: {MaxSize} MB");
            Console.WriteLine($"Memória Usada: {UsedMemory} MB");
            Console.WriteLine($"Memória Livre: {MaxSize - UsedMemory} MB");

            if (_processosNaMemoria.Any())
            {
                Console.WriteLine("\nProcessos/Threads na memória:");
                foreach (var p in _processosNaMemoria)
                {
                    Console.WriteLine($"- {p} ");
                }
            }
            else
            {
                Console.WriteLine("\nNenhum processo na memória.");
            }
        }
    }
}
