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

    public Memoria()
    {
    }
}
