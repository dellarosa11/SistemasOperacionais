using simulador_so.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistemas-Operacionais
{
    internal class Thread
    {
        private int Id { get; set; }
        private int Estado { get; set; } = 0; // 0 - Pronto, 1 - Executando, 2 - Bloqueado
        private bool FoiExecutado { get; set; }

        // Construtor
        public Thread(int id)
        {
            Id = id;
            Estado = 0; // Inicia
            FoiExecutado = false; // Não foi executado ainda
        }

        // Métodos
        public void Executar()
        {
            Estado = 1;
            FoiExecutado = true;
            Console.WriteLine($"Thread - {Id}, Executando");
        }

        public void Bloqueado()
        {
            Estado = 2;
            Console.WriteLine($"Thread - {Id}, Bloqueado");
        }

        public void Desbloquear()
        {
            Estado = 0;
            Console.WriteLine($"Thread - {Id}, Pronta");
        }
    }

}

