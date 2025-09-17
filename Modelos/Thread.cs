using System;
using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Modelos
{
    public class ThreadProcesso : ModeloProcesso
    {
        public string Nome { get; }

        public ThreadProcesso(string nome, ModeloProcesso pai, int prioridade, int memoriaAlocada, int tempoExecucao)
            : base(prioridade, memoriaAlocada, tempoExecucao, pai)
        {
            Nome = nome;
        }

        public override void Executar()
        {
            base.Executar();
            Console.WriteLine($"    Thread '{Nome}' (TID {Id}) do processo PID {Pai?.Id} está executando por {TempoExecucao}ms.");
        }
    }
}
