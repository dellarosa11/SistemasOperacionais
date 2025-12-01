namespace SistemasOperacionais.Modelos
{
    public enum EstadoProcesso
    {
        Pronto = 0,
        Executando = 1,
        Bloqueado = 2,
        Finalizado = 3
    }

    public abstract class ModeloProcesso // Representa o ElementoSO no diagrama UML
    {
        private static int _contadorId = 0;
        public int Id { get; }
        public ModeloProcesso? Pai { get; protected set; }
        public EstadoProcesso Estado { get; protected set; } = EstadoProcesso.Pronto;
        public int Prioridade { get; protected set; }
        public bool FoiExecutado { get; protected set; } = false;
        public int TempoExecucao { get; protected set; } = 0; // ms

        // Campos do PCB/TCB (parte comum)
        public int[] Registradores { get; private set; }
        public int ContadorPrograma { get; private set; } = 0;

        protected ModeloProcesso(int prioridade, int tempoExecucao, ModeloProcesso? pai = null)
        {
            Id = ++_contadorId;
            Prioridade = prioridade;
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


        public virtual string ExibirElemento()
        {
            return $"ID {Id} - Prio: {Prioridade} | Estado: {Estado}";
        }
    }
}
