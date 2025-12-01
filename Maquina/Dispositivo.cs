using SistemasOperacionais.Modelos;
using System.Collections.Generic;

namespace SistemasOperacionais.Maquina
{
    public enum TipoDispositivo
    {
        Bloco,
        Caractere
    }

    public class PedidoIO
    {
        public ModeloProcesso Elemento { get; }
        public string Operacao { get; }
        public int TempoServico { get; }

        public PedidoIO(ModeloProcesso elemento, string operacao, int tempoServico)
        {
            Elemento = elemento;
            Operacao = operacao;
            TempoServico = tempoServico;
        }
    }

    public abstract class Dispositivo
    {
        private static int _contadorId = 0;
        public int Id { get; }
        public string Nome { get; }
        public TipoDispositivo Tipo { get; }
        public Queue<PedidoIO> FilaPedidos { get; } = new Queue<PedidoIO>();
        public bool Ocupado { get; private set; } = false;

        protected Dispositivo(string nome, TipoDispositivo tipo)
        {
            Id = ++_contadorId;
            Nome = nome;
            Tipo = tipo;
        }

        public void AdicionarPedido(PedidoIO pedido)
        {
            FilaPedidos.Enqueue(pedido);
            Console.WriteLine($"[IO] Pedido de {pedido.Operacao} adicionado à fila do {Nome} (ID {Id}) pelo ID {pedido.Elemento.Id}.");
        }

        public void AtenderPedido()
        {
            if (FilaPedidos.Count > 0 && !Ocupado)
            {
                PedidoIO pedido = FilaPedidos.Dequeue();
                Ocupado = true;
                SistemaOperacional.Instancia().Estatisticas.RegistrarUsoDispositivo(Nome);
                Console.WriteLine($"[IO] {Nome} (ID {Id}) começou a atender pedido de {pedido.Operacao} para ID {pedido.Elemento.Id}. Tempo: {pedido.TempoServico}ms.");
                
                // Simulação de tempo de serviço
                // Na simulação por ciclos, o tempo de serviço será decrementado a cada ciclo
                // Por enquanto, apenas simula a conclusão imediata para fins de teste inicial
                
                // Simula a conclusão e gera interrupção (desbloqueia o processo)
                Ocupado = false;
                pedido.Elemento.Desbloquear();
                SistemaOperacional.Instancia().EscalonadorCPU.Adicionar(pedido.Elemento);
                Console.WriteLine($"[IO] {Nome} (ID {Id}) concluiu pedido. ID {pedido.Elemento.Id} desbloqueado.");
            }
        }
    }

    public class DispositivoBloco : Dispositivo
    {
        public DispositivoBloco(string nome) : base(nome, TipoDispositivo.Bloco) { }
    }

    public class DispositivoCaractere : Dispositivo
    {
        public DispositivoCaractere(string nome) : base(nome, TipoDispositivo.Caractere) { }
    }
}
