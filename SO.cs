using SistemasOperacionais.Modelos;
using SistemasOperacionais.Maquina;
using SistemasOperacionais.Programa;
using SistemasOperacionais.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemasOperacionais
{
    internal class SO : ModeloProcesso, IEscalonador , ICriarProcesso
    {
        public List<CPU> CPUs = new();

        public void InicializarCPUs(int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
            {
                CPUs.Add(new CPU());
            }
        }

        public void CriarProcesso()
        {
            // Lógica para criar um novo processo
        }

        public void IniciarProcessos()
        {
            // Lógica para iniciar processos
        }
    }
}