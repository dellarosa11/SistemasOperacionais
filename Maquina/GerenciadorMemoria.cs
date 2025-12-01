using SistemasOperacionais.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Maquina
{
    // Classe para representar uma moldura (frame) de memória física
    public class Moldura
    {
        public int Id { get; }
        public bool Ocupada { get; set; } = false;
        public int ProcessoId { get; set; } = -1; // ID do processo que ocupa a moldura
        public int PaginaId { get; set; } = -1; // ID da página que ocupa a moldura
        public long TempoEntrada { get; set; } // Para política FIFO

        public Moldura(int id)
        {
            Id = id;
        }
    }

    // Classe para representar uma página de memória lógica
    public class Pagina
    {
        public int Id { get; }
        public int MolduraId { get; set; } = -1; // ID da moldura física onde a página está
        public bool Presente { get; set; } = false; // Indica se a página está na memória principal
        public int BlocoSwap { get; set; } = -1; // Bloco no arquivo de paginação (disco)

        public Pagina(int id)
        {
            Id = id;
        }
    }

    // Classe para representar a Tabela de Páginas de um Processo
    public class TabelaPaginas
    {
        public int ProcessoId { get; }
        public List<Pagina> Paginas { get; } = new List<Pagina>();

        public TabelaPaginas(int processoId)
        {
            ProcessoId = processoId;
        }
    }

    public class GerenciadorMemoria
    {
        public int TamanhoPagina { get; } // Em MB
        public int NumMolduras { get; }
        public List<Moldura> MapaMolduras { get; }
        private Dictionary<int, TabelaPaginas> _tabelasPaginas; // Key: ProcessoId

        // Estatísticas
        // A contagem de faltas de página será feita diretamente na classe EstatisticasSO do SistemaOperacional.
        private long _clock = 0; // Simulação de um relógio para FIFO

        public GerenciadorMemoria(int tamanhoPaginaMB, int numMolduras)
        {
            TamanhoPagina = tamanhoPaginaMB;
            NumMolduras = numMolduras;
            MapaMolduras = Enumerable.Range(0, numMolduras).Select(i => new Moldura(i)).ToList();
            _tabelasPaginas = new Dictionary<int, TabelaPaginas>();
        }

        // Aloca memória para um processo (apenas cria a Tabela de Páginas)
        public bool Alocar(Processo processo)
        {
            // Calcula o número de páginas necessárias
            int paginasNecessarias = (int)Math.Ceiling((double)processo.MemoriaAlocada / TamanhoPagina);

            if (paginasNecessarias == 0) return true; // Não precisa de memória

            // Cria a Tabela de Páginas (todas as páginas inicialmente não estão presentes)
            var tabela = new TabelaPaginas(processo.Id);
            for (int i = 0; i < paginasNecessarias; i++)
            {
                Pagina pagina = new Pagina(i) { Presente = false };
                tabela.Paginas.Add(pagina);
            }

            _tabelasPaginas.Add(processo.Id, tabela);
            Console.WriteLine($"[Memória] Criada Tabela de Páginas para PID {processo.Id} com {paginasNecessarias} páginas.");
            return true;
        }

        // Desaloca a memória de um processo
        public void Desalocar(Processo processo)
        {
            if (_tabelasPaginas.TryGetValue(processo.Id, out var tabela))
            {
                foreach (var pagina in tabela.Paginas)
                {
                    if (pagina.Presente)
                    {
                        // Libera a moldura
                        MapaMolduras[pagina.MolduraId].Ocupada = false;
                        MapaMolduras[pagina.MolduraId].ProcessoId = -1;
                        MapaMolduras[pagina.MolduraId].PaginaId = -1;
                    }
                    // TODO: Desalocar bloco de swap se BlocoSwap != -1
                }
                _tabelasPaginas.Remove(processo.Id);
                Console.WriteLine($"[Memória] Desalocado Tabela de Páginas para PID {processo.Id}.");
            }
        }

        // Simulação de tradução de endereço lógico para físico
        // Endereço lógico: (Número da Página, Offset)
        // Retorna o ID da moldura física
        public int TraduzirEndereco(int processoId, int numeroPagina)
        {
            _clock++;

            if (_tabelasPaginas.TryGetValue(processoId, out var tabela))
            {
                if (numeroPagina >= 0 && numeroPagina < tabela.Paginas.Count)
                {
                    Pagina pagina = tabela.Paginas[numeroPagina];

                    if (!pagina.Presente)
                    {
                        // FALTA DE PÁGINA
                        SistemaOperacional.Instancia().Estatisticas.FaltasDePagina++;
                        Console.WriteLine($"[Memória] FALTA DE PÁGINA para PID {processoId}, Página {numeroPagina}.");

                        // 1. Encontra uma moldura livre ou vítima
                        Moldura molduraLivre = MapaMolduras.FirstOrDefault(m => !m.Ocupada);

                        if (molduraLivre == null)
                        {
                            // 1.1. Substituição de Página (FIFO)
                            molduraLivre = SubstituirPaginaFIFO();
                            
                            // 1.2. Swap-out (se a página vítima estiver suja, o que não simulamos aqui)
                            // Simulação: Apenas desaloca a moldura da página vítima
                            Pagina paginaVitima = _tabelasPaginas[molduraLivre.ProcessoId].Paginas[molduraLivre.PaginaId];
                            paginaVitima.Presente = false;
                            paginaVitima.MolduraId = -1;
                            Console.WriteLine($"[Memória] SWAP-OUT: Página {paginaVitima.Id} do PID {molduraLivre.ProcessoId} removida da Moldura {molduraLivre.Id}.");
                            SistemaOperacional.Instancia().Estatisticas.SwapOuts++;
                            
                            // A moldura vítima deve ser marcada como livre para ser reutilizada
                            molduraLivre.Ocupada = false;
                            molduraLivre.ProcessoId = -1;
                            molduraLivre.PaginaId = -1;
                        }

                        // 2. Swap-in (carrega a página necessária)
                        // Agora, garantimos que molduraLivre está livre (ou foi liberada pelo swap-out)
                        // Se molduraLivre veio do SubstituirPaginaFIFO, ela já foi liberada e deve ser a moldura a ser usada.
                        // Se molduraLivre veio do FirstOrDefault, ela já estava livre.
                        
                        // A moldura a ser usada é molduraLivre.
                        
                        pagina.Presente = true;
                        pagina.MolduraId = molduraLivre.Id;
                        
                        molduraLivre.Ocupada = true;
                        molduraLivre.ProcessoId = processoId;
                        molduraLivre.PaginaId = numeroPagina;
                        molduraLivre.TempoEntrada = _clock;
                        
                        Console.WriteLine($"[Memória] SWAP-IN: Página {numeroPagina} do PID {processoId} carregada na Moldura {molduraLivre.Id}.");
                        SistemaOperacional.Instancia().Estatisticas.SwapIns++;
                    }

                    // 3. Retorna a moldura
                    return pagina.MolduraId;
                }
            }
            throw new Exception($"[Memória] Erro de Segmentação/Paginação para PID {processoId}.");
        }

        private Moldura SubstituirPaginaFIFO()
        {
            // Encontra a moldura com o menor TempoEntrada (FIFO)
            Moldura molduraVitima = MapaMolduras.OrderBy(m => m.TempoEntrada).First();
            return molduraVitima;
        }
    }
}
