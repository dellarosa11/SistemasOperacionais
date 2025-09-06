using SistemasOperacionais.Modelos;
using SistemasOperacionais.Maquina;
using SistemasOperacionais.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemasOperacionais.Programa;
{
    public class Thread : Modelos.ModeloProcesso
    {

        // Construtor - A ser implementado
        public Thread(ModeloProcesso pai, int id, int estado, int prioridade, bool foiexecutado, int memalocada, int tempoexecucao)
        : base(pai, id, estado, prioridade, foiexecutado, memalocada, tempoexecucao)
        {
        }

        // Métodos
        public override void Executar()
        {
            estado = 1;
            foiexecutado = true;
            Console.WriteLine($"Thread - {id}, Executando");
        }

        public override void Bloqueado()
        {
            estado = 2;
            Console.WriteLine($"Thread - {id}, Bloqueado");
        }

        public override void Desbloquear()
        {
            estado = 0;
            Console.WriteLine($"Thread - {id}, Pronta");
        }
    }

}

