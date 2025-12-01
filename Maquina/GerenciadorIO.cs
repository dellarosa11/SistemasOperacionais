using SistemasOperacionais.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Maquina
{
    public class GerenciadorIO
    {
        public List<Dispositivo> Dispositivos { get; } = new List<Dispositivo>();

        public GerenciadorIO()
        {
            // Inicializa dispositivos de exemplo
            Dispositivos.Add(new DispositivoBloco("Disco Rígido 1"));
            Dispositivos.Add(new DispositivoCaractere("Teclado"));
        }

        public Dispositivo? ObterDispositivo(string nome)
        {
            return Dispositivos.FirstOrDefault(d => d.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public void SolicitarIO(ModeloProcesso elemento, string nomeDispositivo, string operacao, int tempoServico)
        {
            Dispositivo? dispositivo = ObterDispositivo(nomeDispositivo);

            if (dispositivo != null)
            {
                // 1. Bloqueia o elemento
                elemento.Bloquear();

                // 2. Cria o pedido e adiciona à fila do dispositivo
                PedidoIO pedido = new PedidoIO(elemento, operacao, tempoServico);
                dispositivo.AdicionarPedido(pedido);
            }
            else
            {
                Console.WriteLine($"[IO] Dispositivo '{nomeDispositivo}' não encontrado.");
            }
        }

        public void ProcessarIO()
        {
            // Simula o processamento de IO em um ciclo
            foreach (var dispositivo in Dispositivos)
            {
                dispositivo.AtenderPedido();
            }
        }
    }
}
