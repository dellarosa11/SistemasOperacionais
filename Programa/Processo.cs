using SistemasOperacionais.Modelo;
using SistemasOperacionais.Maquina;
using SistemasOperacionais.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais.Programa;


public class Processo
{
    public string Nome { get; set; }
    public int Tamanho { get; set; } // em MB

}
public class Processo : Modelos.ModeloProcesso
{
    public string Nome { get; private set; }

    public Processo(string nome, int prioridade, int memoria, int tempo)
            : base(prioridade, memoria, tempo)
        {
            Nome = nome;
        }

    public override void Executar()
        {
            base.Executar();
            Console.WriteLine($"Processo {Nome} começou a rodar por {TempoExecucao}ms.");
        }

    public override string ToString()
        {
            return $"[PID {Id}] {Nome} | Estado: {Estado} | Memória: {MemoriaAlocada}MB | Prioridade: {Prioridade}";
        }
    }