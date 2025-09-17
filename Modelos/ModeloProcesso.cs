using System;

namespace SistemasOperacionais.Modelos
{
    public enum EstadoProcesso
    {
        Pronto = 0,
        Executando = 1,
        Bloqueado = 2,
        Finalizado = 3
    }

    public abstract class ModeloProcesso
    {
        private static int _contadorId = 0;
        public int Id { get; }
        public ModeloProcesso? Pai { get; protected set; }
        public EstadoProcesso Estado { get; protected set; } = EstadoProcesso.Pronto;
        public int Prioridade { get; protected set; }
        public bool FoiExecutado { get; protected set; } = false;
        public int MemoriaAlocada { get; protected set; } = 0; // MB
        public int TempoExecucao { get; protected set; } = 0; // ms

        protected ModeloProcesso(int prioridade, int memoriaAlocada, int tempoExecucao, ModeloProcesso? pai = null)
        {
            Id = ++_contadorId;
            Prioridade = prioridade;
            MemoriaAlocada = memoriaAlocada;
            TempoExecucao = tempoExecucao;
            Pai = pai;
        }

        public virtual void Executar()
        {
            Estado = EstadoProcesso.Executando;
            FoiExecutado = true;
            Console.WriteLine($"[PID {Id}] Estado: {Estado} - iniciando execução por {TempoExecucao}ms.");
        }

        public virtual void Bloquear()
        {
            Estado = EstadoProcesso.Bloqueado;
            Console.WriteLine($"[PID {Id}] Bloqueado.");
        }

        public virtual void Desbloquear()
        {
            Estado = EstadoProcesso.Pronto;
            Console.WriteLine($"[PID {Id}] Desbloqueado e pronto.");
        }

        public virtual void Finalizar()
        {
            Estado = EstadoProcesso.Finalizado;
            Console.WriteLine($"[PID {Id}] Finalizado.");
        }

        public override string ToString()
        {
            return $"[PID {Id}] Estado: {Estado} | Memória: {MemoriaAlocada}MB | Prioridade: {Prioridade}";
        }
    }
}
