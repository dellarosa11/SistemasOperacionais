using SistemasOperacionais.Maquina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SistemasOperacionais.Modelos
{
    public enum EstadoProcesso
    {
        Pronto = 0,
        Executando = 1,
        Bloqueado = 2
    }

    public abstract class ModeloProcesso
    {
        public ModeloProcesso? Pai { get; protected set; } = null;
        public int Id { get; protected set; }
        public EstadoProcesso Estado { get; protected set; } = EstadoProcesso.Pronto;
        public int Prioridade { get; protected set; }
        public bool FoiExecutado { get; protected set; } = false;
        public int MemoriaAlocada { get; protected set; } = 0;
        public int TempoExecucao { get; protected set; } = 0;

        private static int _contadorId = 0;

        protected ModeloProcesso(int prioridade, int memoriaAlocada, int tempoExecucao)
        {
            Id = ++_contadorId;
            Prioridade = prioridade;
            MemoriaAlocada = memoriaAlocada;
            TempoExecucao = tempoExecucao;
        }

        public virtual void Executar()
        {
            Estado = EstadoProcesso.Executando;
            FoiExecutado = true;
            Console.WriteLine($"[PID {Id}] Executando...");
        }

        public virtual void Bloquear()
        {
            Estado = EstadoProcesso.Bloqueado;
            Console.WriteLine($"[PID {Id}] Bloqueado.");
        }

        public virtual void Desbloquear()
        {
            Estado = EstadoProcesso.Pronto;
            Console.WriteLine($"[PID {Id}] Pronto para execução.");
        }
    }
}
