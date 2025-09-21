namespace SistemasOperacionais.Modelos
{
    public enum EstadoProcesso
    {
        Pronto = 0,
        Executando = 1,
        Bloqueado = 2,
        Finalizado = 3
    }

    public abstract class ModeloProcesso
    {
        private static int _contadorId = 0;
        public int Id { get; }
        public ModeloProcesso? Pai { get; protected set; }
        public EstadoProcesso Estado { get; protected set; } = EstadoProcesso.Pronto;
        public int Prioridade { get; protected set; }
        public bool FoiExecutado { get; protected set; } = false;
        public int MemoriaAlocada { get; protected set; } = 0; // MB
        public int TempoExecucao { get; protected set; } = 0; // ms

        // Novos Campos
        public int[] Registradores { get; private set; }
        public int ContadorPrograma { get; private set; } = 0;
        public List<string> ArquivosAbertos { get; private set; } = new List<string>();

        protected ModeloProcesso(int prioridade, int memoriaAlocada, int tempoExecucao, ModeloProcesso? pai = null)
        {
            Id = ++_contadorId;
            Prioridade = prioridade;
            MemoriaAlocada = memoriaAlocada;
            TempoExecucao = tempoExecucao;
            Pai = pai;

            // 8 registradores simulados
            Registradores = new int[8];
        }

        public virtual void Executar()
        {
            Estado = EstadoProcesso.Executando;
            FoiExecutado = true;
            ContadorPrograma += TempoExecucao;
        }

        public virtual void Bloquear()
        {
            Estado = EstadoProcesso.Bloqueado;
            Console.WriteLine($"[PID {Id}] Bloqueado.");
        }

        public virtual void Desbloquear()
        {
            Estado = EstadoProcesso.Pronto;
            Console.WriteLine($"[PID {Id}] Desbloqueado e pronto.");
        }

        public virtual void Finalizar()
        {
            Estado = EstadoProcesso.Finalizado;
        }

        // Manipulação de arquivos simulada
        public void AbrirArquivo(string nome)
        {
            ArquivosAbertos.Add(nome);
            Console.WriteLine($"[PID {Id}] abriu arquivo {nome}");
        }

        public void FecharArquivo(string nome)
        {
            if (ArquivosAbertos.Remove(nome))
                Console.WriteLine($"[PID {Id}] fechou arquivo {nome}");
        }

        public virtual string ExibirProcesso()
        {
            return $"PID {Id} - Mem: {MemoriaAlocada}MB - Prio: {Prioridade}";
        }
    }
}
