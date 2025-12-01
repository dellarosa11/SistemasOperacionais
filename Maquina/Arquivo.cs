using System;
using System.Collections.Generic;

namespace SistemasOperacionais.Maquina
{
    public class Arquivo
    {
        public string Nome { get; set; }
        public int Tamanho { get; private set; } // Em blocos (simulados)
        public DateTime TimestampCriacao { get; }
        public DateTime TimestampModificacao { get; private set; }
        public string Permissoes { get; set; } // Ex: "rw-r--r--"
        public List<int> BlocosAlocados { get; private set; } // Simulação de alocação encadeada

        public Arquivo(string nome, string permissoes = "rw-r--r--")
        {
            Nome = nome;
            Tamanho = 0;
            TimestampCriacao = DateTime.Now;
            TimestampModificacao = DateTime.Now;
            Permissoes = permissoes;
            BlocosAlocados = new List<int>();
        }

        public void Escrever(int numBlocos, List<int> novosBlocos)
        {
            // Simulação de escrita: apenas atualiza o tamanho e a lista de blocos
            Tamanho += numBlocos;
            BlocosAlocados.AddRange(novosBlocos);
            TimestampModificacao = DateTime.Now;
        }

        public void Ler()
        {
            // Simulação de leitura
            Console.WriteLine($"[FS] Lendo arquivo '{Nome}'. Blocos: {string.Join(", ", BlocosAlocados)}");
        }

        public void Truncar()
        {
            Tamanho = 0;
            BlocosAlocados.Clear();
            TimestampModificacao = DateTime.Now;
        }
    }

    public class Diretorio
    {
        public string Nome { get; }
        public Diretorio? Pai { get; }
        public List<Diretorio> Subdiretorios { get; } = new List<Diretorio>();
        public List<Arquivo> Arquivos { get; } = new List<Arquivo>();

        public Diretorio(string nome, Diretorio? pai = null)
        {
            Nome = nome;
            Pai = pai;
        }

        public void Adicionar(Arquivo arquivo)
        {
            Arquivos.Add(arquivo);
        }

        public void Adicionar(Diretorio diretorio)
        {
            Subdiretorios.Add(diretorio);
        }

        public Arquivo? ObterArquivo(string nome)
        {
            return Arquivos.Find(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public Diretorio? ObterDiretorio(string nome)
        {
            return Subdiretorios.Find(d => d.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public void Listar()
        {
            Console.WriteLine($"[FS] Conteúdo do diretório '{Nome}':");
            foreach (var dir in Subdiretorios)
            {
                Console.WriteLine($"  <DIR> {dir.Nome}");
            }
            foreach (var arq in Arquivos)
            {
                Console.WriteLine($"  <FILE> {arq.Nome} ({arq.Tamanho} blocos)");
            }
        }
    }
}
