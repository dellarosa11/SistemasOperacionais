using SistemasOperacionais.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Maquina
{
    public class GerenciadorArquivos
    {
        public Diretorio Raiz { get; }
        private const int TAMANHO_BLOCO = 4; // Simulação: 4KB por bloco
        private const int NUM_BLOCOS = 1024; // Simulação: 1024 blocos (4MB total)
        private bool[] _mapaDeBlocos; // Simulação de FAT/Mapa de Bits

        // Tabela Global de Arquivos Abertos (GAT)
        private List<Arquivo> _arquivosAbertosGlobal = new List<Arquivo>();

        public GerenciadorArquivos()
        {
            Raiz = new Diretorio("/");
            _mapaDeBlocos = new bool[NUM_BLOCOS];
            Console.WriteLine($"[FS] Sistema de Arquivos inicializado. Tamanho total: {NUM_BLOCOS * TAMANHO_BLOCO}KB.");
        }

        // Simulação de alocação contígua ou encadeada (aqui, apenas aloca blocos livres)
        private List<int> AlocarBlocos(int numBlocos)
        {
            List<int> blocosAlocados = new List<int>();
            for (int i = 0; i < NUM_BLOCOS && blocosAlocados.Count < numBlocos; i++)
            {
                if (!_mapaDeBlocos[i])
                {
                    _mapaDeBlocos[i] = true;
                    blocosAlocados.Add(i);
                }
            }

            if (blocosAlocados.Count < numBlocos)
            {
                // Desaloca os blocos que foram alocados
                foreach (var bloco in blocosAlocados)
                {
                    _mapaDeBlocos[bloco] = false;
                }
                return new List<int>(); // Falha na alocação
            }

            return blocosAlocados;
        }

        private void DesalocarBlocos(List<int> blocos)
        {
            foreach (var bloco in blocos)
            {
                if (bloco >= 0 && bloco < NUM_BLOCOS)
                {
                    _mapaDeBlocos[bloco] = false;
                }
            }
        }

        public Arquivo? CriarArquivo(string nome, Diretorio diretorio)
        {
            if (diretorio.ObterArquivo(nome) != null)
            {
                Console.WriteLine($"[FS] Erro: Arquivo '{nome}' já existe em '{diretorio.Nome}'.");
                return null;
            }

            Arquivo novoArquivo = new Arquivo(nome);
            diretorio.Adicionar(novoArquivo);
            Console.WriteLine($"[FS] Arquivo '{nome}' criado em '{diretorio.Nome}'.");
            return novoArquivo;
        }

        public Arquivo? AbrirArquivo(string nome, Processo processo)
        {
            // Simulação: Procura o arquivo na raiz (simplificação)
            Arquivo? arquivo = Raiz.ObterArquivo(nome);

            if (arquivo != null)
            {
                // Adiciona à Tabela Global de Arquivos Abertos (GAT)
                _arquivosAbertosGlobal.Add(arquivo);
                // Adiciona à Tabela de Arquivos Abertos do Processo (FAT)
                processo.ArquivosAbertos.Add(nome);
                Console.WriteLine($"[FS] Arquivo '{nome}' aberto por PID {processo.Id}.");
                return arquivo;
            }

            Console.WriteLine($"[FS] Erro: Arquivo '{nome}' não encontrado.");
            return null;
        }

        public void FecharArquivo(string nome, Processo processo)
        {
            // Remove da Tabela de Arquivos Abertos do Processo (FAT)
            if (processo.ArquivosAbertos.Remove(nome))
            {
                // Remove da Tabela Global de Arquivos Abertos (GAT)
                Arquivo? arquivo = _arquivosAbertosGlobal.Find(a => a.Nome.Equals(nome));
                if (arquivo != null)
                {
                    _arquivosAbertosGlobal.Remove(arquivo);
                }
                Console.WriteLine($"[FS] Arquivo '{nome}' fechado por PID {processo.Id}.");
            }
            else
            {
                Console.WriteLine($"[FS] Erro: Arquivo '{nome}' não estava aberto por PID {processo.Id}.");
            }
        }

        public bool EscreverArquivo(string nome, Processo processo, int tamanhoKB)
        {
            if (!processo.ArquivosAbertos.Contains(nome))
            {
                Console.WriteLine($"[FS] Erro: Arquivo '{nome}' não está aberto por PID {processo.Id}.");
                return false;
            }

            Arquivo? arquivo = Raiz.ObterArquivo(nome);
            if (arquivo == null) return false;

            int numBlocos = (int)Math.Ceiling((double)tamanhoKB / TAMANHO_BLOCO);
            List<int> blocos = AlocarBlocos(numBlocos);

            if (blocos.Count == numBlocos)
            {
                arquivo.Escrever(numBlocos, blocos);
                Console.WriteLine($"[FS] PID {processo.Id} escreveu {tamanhoKB}KB em '{nome}'. Alocado {numBlocos} blocos.");
                return true;
            }
            else
            {
                Console.WriteLine($"[FS] Erro: Falha ao alocar blocos para escrever em '{nome}'.");
                return false;
            }
        }

        public void ApagarArquivo(string nome, Diretorio diretorio)
        {
            Arquivo? arquivo = diretorio.ObterArquivo(nome);
            if (arquivo != null)
            {
                DesalocarBlocos(arquivo.BlocosAlocados);
                diretorio.Arquivos.Remove(arquivo);
                Console.WriteLine($"[FS] Arquivo '{nome}' apagado.");
            }
            else
            {
                Console.WriteLine($"[FS] Erro: Arquivo '{nome}' não encontrado para apagar.");
            }
        }

        public void ListarDiretorio(Diretorio diretorio)
        {
            diretorio.Listar();
        }
    }
}
