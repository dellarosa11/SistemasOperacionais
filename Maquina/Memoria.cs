using SistemasOperacionais.Maquina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;


namespace SistemasOperacionais.Maquina;

public class Memoria
{
    public int MaxSize { get; private set; } = 1024; //MB 
    public int UsedMemory { get; private set; } = 0;
    private List<Processo> ProcessosNaMemoria = new List<Processo>();
    //estou criando um monstro aqui

    public Memoria(maxSize int)
    {
        MaxSize = maxSize; // tenho que trocar as variaveis
        UsedMemory = usedMemory; // tenho que trocar as variaveis
    }
    public bool AlocarMemoria(Processo processo)
    {
        if (UsedMemory + processo.Tamanho <= MaxSize)
        {
            ProcessosNaMemoria.Add(processo);
            UsedMemory += processo.Tamanho;
            return true;
        }
        return false;
    }
    public void LiberarMemoria(Processo processo)
    {
        if (ProcessosNaMemoria.Contains(processo))
        {
            ProcessosNaMemoria.Remove(processo);
            UsedMemory -= processo.Tamanho;
        }
    }
    public void MostrarEstadoMemoria()

}
