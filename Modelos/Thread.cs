namespace SistemasOperacionais.Modelos
{
    public class ThreadProcesso : ModeloProcesso
    {
        public string Nome { get; }
        public Processo ProcessoPai { get; }

        public ThreadProcesso(string nome, Processo pai, int prioridade, int tempoExecucao)
            : base(prioridade, tempoExecucao, pai)
        {
            Nome = nome;
            ProcessoPai = pai;
        }

        public override void Executar()
        {
            base.Executar();
            Console.WriteLine($"    Thread '{Nome}' (TID {Id}) do processo PID {ProcessoPai.Id} está executando por {TempoExecucao}ms.");
        }
    }
}
