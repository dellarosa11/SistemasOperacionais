namespace SistemasOperacionais.Modelos
{
    public class Processo : ModeloProcesso
    {
        public string Nome { get; }

        public Processo(string nome, int prioridade, int memoriaAlocada, int tempoExecucao, ModeloProcesso? pai = null)
            : base(prioridade, memoriaAlocada, tempoExecucao, pai)
        {
            Nome = nome;
        }

        public override void Executar()
        {
            base.Executar();
            Console.WriteLine($"Processo '{Nome}' (PID {Id}) está executando por {TempoExecucao}ms.");
            // Simulação simples: apenas mensurar; tempo real pode ser simulado externamente
        }

        public override string ExibirProcesso()
        {
            return $"[PID {Id}] {Nome} | Estado: {Estado} | Memória: {MemoriaAlocada}MB | Prioridade: {Prioridade} | ID do Pai: {Pai.Id}";
        }
    }
}
