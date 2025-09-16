using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais.Interfaces
{
    public interface ICriarProcesso
    {
        public void CriarProcesso()
        {
            Console.WriteLine("Criando novo processo...");
        }
        public void IniciarProcessos()
        { 
            Console.WriteLine("Iniciando processos...");

        }
    }
}
