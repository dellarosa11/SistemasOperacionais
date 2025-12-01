using SistemasOperacionais.Interfaces;
using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Escalonadores
{
    public class RoundRobinEscalonador : IEscalonador
    {
        private readonly Queue<ModeloProcesso> _filaProntos = new Queue<ModeloProcesso>();
        private readonly int _quantum;

        public RoundRobinEscalonador(int quantum = 20)
        {
            _quantum = quantum;
        }

        public void Adicionar(ModeloProcesso elemento)
        {
            _filaProntos.Enqueue(elemento);
        }

        public ModeloProcesso? Proximo()
        {
            if (_filaProntos.Count == 0)
            {
                return null;
            }

            // O Round Robin simplesmente pega o primeiro da fila
            return _filaProntos.Dequeue();
        }

        // TODO: Adicionar lógica de quantum e preempção no SistemaOperacional
    }
}
