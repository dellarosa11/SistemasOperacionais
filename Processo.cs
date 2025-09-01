using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistemas-Operacionais;
{
    internal class Processo
    {
        private Processo? Pai { get; set; } = null;
        private int Id { get; set; }
        private int Estado { get; set; } = 0; // 0 - Pronto, 1 - Executando, 2 - Bloqueado
        private int Prioridade { get; set; }
        private bool FoiExecutado { get; set; } = false;
        private int MemoriaAlocada { get; set; } = 0;
        private int TempoExecucao { get; set; } = 0;

        public Processo(int id, int prioridade, Processo? pai = null)
        {
            Id = id;
            Prioridade = prioridade;
            Pai = pai;
        }


    }
}
