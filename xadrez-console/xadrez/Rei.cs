using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor)
        {
        }

        public bool PodeMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.GetPeca(posicao);
            return peca == null || peca.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao posicao = new Posicao(0, 0);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    posicao.DefinirValores(Posicao.Linha + i, Posicao.Coluna + j);
                    if (Tabuleiro.PosicaoValida(posicao) && PodeMover(posicao))
                        matriz[posicao.Linha, posicao.Coluna] = true;
                }
            }

            return matriz;
        }

        public override string ToString()
        {
            return "R";
        }


    }
}
