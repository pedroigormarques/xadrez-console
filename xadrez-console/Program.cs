using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main()
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    try
                    {

                        Console.Clear();
                        Tela.ImprimirPartida(partida);

                        Console.Write("\nOrigem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        Console.Clear();
                        bool[,] posicoesPossiveis = partida.Tabuleiro.GetPeca(origem).MovimentosPossiveis();
                        Tela.ImprimirTabuleiro(partida.Tabuleiro, posicoesPossiveis);
                        //Console.WriteLine("\nTurno: " + partida.Turno);
                        //Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);

                        Console.Write("\nDestino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        partida.RealizarJogada(origem, destino);

                    }
                    catch (TabuleiroException erro)
                    {
                        Console.WriteLine(erro.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Tela.ImprimirPartida(partida);
            }
            catch (TabuleiroException erro)
            {
                Console.WriteLine("Erro: " + erro.Message);
            }



        }
    }
}
