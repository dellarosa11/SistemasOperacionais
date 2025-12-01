using SistemasOperacionais.Maquina;

namespace SistemasOperacionais.Modelos
{
    public class Processo : ModeloProcesso
    {
        public string Nome { get; }

        public int MemoriaAlocada { get; protected set; } = 0; // MB
        public List<string> ArquivosAbertos { get; private set; } = new List<string>();

        public Processo(string nome, int prioridade, int memoriaAlocada, int tempoExecucao, Processo? pai = null)
            : base(prioridade, tempoExecucao, pai)
        {
            Nome = nome;
            MemoriaAlocada = memoriaAlocada;
        }

        public void AbrirArquivo(string nome)
        {
            SistemaOperacional.Instancia().GerenciadorArquivos.AbrirArquivo(nome, this);
        }

        public void FecharArquivo(string nome)
        {
            SistemaOperacional.Instancia().GerenciadorArquivos.FecharArquivo(nome, this);
        }

        public void EscreverArquivo(string nome, int tamanhoKB)
        {
            SistemaOperacional.Instancia().GerenciadorArquivos.EscreverArquivo(nome, this, tamanhoKB);
        }

        // Simulação de acesso à memória para testar a paginação
        public void AcessarMemoria(int numeroPagina)
        {
            if (numeroPagina >= 0 && numeroPagina < (int)Math.Ceiling((double)MemoriaAlocada / SistemaOperacional.Instancia().GerenciadorMemoria.TamanhoPagina))
            {
                SistemaOperacional.Instancia().GerenciadorMemoria.TraduzirEndereco(Id, numeroPagina);
            }
            else
            {
                Console.WriteLine($"[PID {Id}] Erro: Tentativa de acesso à página {numeroPagina} fora do limite de memória alocada.");
            }
        }

        // Simulação de chamada de sistema para IO
        public void ChamarIO(string dispositivo, string operacao, int tempoServico)
        {
            Console.WriteLine($"[PID {Id}] Chamada de sistema para IO: {operacao} em {dispositivo}.");
            SistemaOperacional.Instancia().GerenciadorIO.SolicitarIO(this, dispositivo, operacao, tempoServico);
        }

        public override void Executar()
        {
            base.Executar();
            Console.WriteLine($"Processo '{Nome}' (PID {Id}) está executando por {TempoExecucao}ms.");
            // Simulação simples: apenas mensurar; tempo real pode ser simulado externamente
        }

        public override string ExibirElemento()
        {
            string paiId = Pai != null ? Pai.Id.ToString() : "N/A";
            return $"[PID {Id}] {Nome} | Estado: {Estado} | Memória: {MemoriaAlocada}MB | Prioridade: {Prioridade} | ID do Pai: {paiId}";
        }
    }
}
