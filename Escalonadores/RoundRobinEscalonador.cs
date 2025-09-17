using System.Collections.Generic;
using System.Linq;
using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Escalonadores
{
    public class RoundRobinEscalonador : IEscalonador
    {
        private int _ultimoIndex = -1;

        public ModeloProcesso? DecidirProximoProcesso(IList<ModeloProcesso> prontos)
        {
            if (prontos == null || prontos.Count == 0) return null;

            // Avança circularmente
            _ultimoIndex = (_ultimoIndex + 1) % prontos.Count;
            var escolhido = prontos[_ultimoIndex];
            return escolhido;
        }
    }
}
