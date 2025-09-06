using SistemasOperacionais.Maquina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemasOperacionais.Modelos;
{
    public abstract class ModeloProcesso
    {
        private ModeloProcesso? Pai { get; set; } = null;
        private int Id { get; set; }
        private int Estado { get; set; } = 0; // 0 - Pronto, 1 - Executando, 2 - Bloqueado
        private int Prioridade { get; set; }
        private bool FoiExecutado { get; set; } = false;
        private int MemoriaAlocada { get; set; } = 0;
        private int TempoExecucao { get; set; } = 0;

        public virtual void Executar()
        {
            Estado = 1;
            FoiExecutado = true;
            Console.WriteLine($"Processo - {Id}, Executando");
        }

        public virtual void Bloqueado()
        {
            Estado = 2;
            Console.WriteLine($"Processo - {Id}, Bloqueado");
        }

        public virtual void Desbloquear()
        {
            Estado = 0;
            Console.WriteLine($"Processo - {Id}, Pronta");
        }

    }
}
