using SistemasOperacionais.Modelos;

namespace SistemasOperacionais.Maquina
{
    public class BlocoMemoria
    {
        public int Inicio { get; set; }
        public int Tamanho { get; set; }
        public ModeloProcesso? Processo { get; set; } // null = livre

        public bool Livre => Processo == null;
    }

    public class Memoria
    {
        public int MaxSize { get; private set; }
        private readonly List<BlocoMemoria> _blocos = new List<BlocoMemoria>();

        public Memoria(int maxSize = 1024)
        {
            if (maxSize <= 0) throw new ArgumentException("maxSize precisa ser > 0");
            MaxSize = maxSize;

            // começa com 1 bloco livre (toda a memória disponível)
            _blocos.Add(new BlocoMemoria { Inicio = 0, Tamanho = maxSize, Processo = null });
        }

        /// <summary>
        /// Aloca memória para um processo usando First Fit
        /// </summary>
        public bool AlocarMemoria(ModeloProcesso processo)
        {
            if (processo == null) throw new ArgumentNullException(nameof(processo));

            // procura o primeiro bloco livre grande o suficiente
            var blocoLivre = _blocos.FirstOrDefault(b => b.Livre && b.Tamanho >= processo.MemoriaAlocada);
            if (blocoLivre == null)
                return false;

            // cria bloco ocupado
            var blocoOcupado = new BlocoMemoria
            {
                Inicio = blocoLivre.Inicio,
                Tamanho = processo.MemoriaAlocada,
                Processo = processo
            };

            // ajusta o bloco livre restante
            blocoLivre.Inicio += processo.MemoriaAlocada;
            blocoLivre.Tamanho -= processo.MemoriaAlocada;

            // insere antes do bloco livre (para manter ordem)
            int index = _blocos.IndexOf(blocoLivre);
            _blocos.Insert(index, blocoOcupado);

            // se o bloco livre zerou, remove
            if (blocoLivre.Tamanho == 0)
                _blocos.Remove(blocoLivre);

            return true;
        }

        /// <summary>
        /// Libera memória de um processo
        /// </summary>
        public void LiberarMemoria(ModeloProcesso processo)
        {
            if (processo == null) return;

            var bloco = _blocos.FirstOrDefault(b => b.Processo == processo);
            if (bloco == null) return;

            bloco.Processo = null; // marca como livre

            // tenta juntar com vizinhos livres (coalescência)
            JuntarBlocosLivres();
        }

        private void JuntarBlocosLivres()
        {
            for (int i = 0; i < _blocos.Count - 1; i++)
            {
                var atual = _blocos[i];
                var proximo = _blocos[i + 1];

                if (atual.Livre && proximo.Livre)
                {
                    atual.Tamanho += proximo.Tamanho;
                    _blocos.RemoveAt(i + 1);
                    i--; // volta um passo para verificar novamente
                }
            }
        }

        public IReadOnlyList<ModeloProcesso> ProcessosNaMemoria =>
            _blocos.Where(b => !b.Livre && b.Processo != null).Select(b => b.Processo!).ToList();

        public void MostrarEstadoMemoria()
        {
            int usada = _blocos.Where(b => !b.Livre).Sum(b => b.Tamanho);
            Console.WriteLine($"\nMemória: {MaxSize}MB | Usada: {usada}MB | Livre: {MaxSize - usada}MB");

            Console.WriteLine("Blocos:");
            foreach (var b in _blocos)
            {
                if (b.Livre)
                    Console.WriteLine($"  [LIVRE {b.Tamanho}MB]");
                else
                    Console.WriteLine($"  [{b.Processo}]");
            }
        }


    }
}
