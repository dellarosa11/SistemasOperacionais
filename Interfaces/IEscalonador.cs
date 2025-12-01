using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Interfaces
{
    public interface IEscalonador
    {
        /// Adiciona um processo ou thread à fila de prontos.
        void Adicionar(ModeloProcesso elemento);

        /// Retorna o próximo elemento (processo ou thread) a ser executado.
        ModeloProcesso? Proximo();
    }
}
