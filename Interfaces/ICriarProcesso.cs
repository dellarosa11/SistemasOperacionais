using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Interfaces
{
    public interface ICriarProcesso
    {
        /// Cria um conjunto inicial de processos (ou um único processo)
        IEnumerable<ModeloProcesso> CriarProcessos();
    }
}
