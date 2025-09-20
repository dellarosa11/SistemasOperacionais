using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Interfaces
{
    public interface ICriarProcesso
    {
        /// <summary>
        /// Cria um conjunto inicial de processos (ou um único processo)
        /// </summary>
        IEnumerable<ModeloProcesso> CriarProcessos();
    }
}
