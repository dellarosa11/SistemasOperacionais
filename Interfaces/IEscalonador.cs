using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Interfaces
{
    public interface IEscalonador
    {
        /// <summary>
        /// Decide o próximo processo a executar a partir da lista de prontos.
        /// Retorna null se não houver processo pronto.
        /// </summary>
        ModeloProcesso? DecidirProximoProcesso(IList<ModeloProcesso> prontos);
    }
}
