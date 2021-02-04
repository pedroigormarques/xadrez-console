using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            ColocarPecas();
        }

        private void ColocarPecas()
        {
            Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Branca), new PosicaoXadrez('a', 1).ToPosicao());
            Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Branca), new PosicaoXadrez('h', 1).ToPosicao());
            Tabuleiro.ColocarPeca(new Rei(Tabuleiro, Cor.Branca), new PosicaoXadrez('e', 1).ToPosicao());


            Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Preta), new PosicaoXadrez('a', 8).ToPosicao());
            Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Preta), new PosicaoXadrez('h', 8).ToPosicao());
            Tabuleiro.ColocarPeca(new Rei(Tabuleiro, Cor.Preta), new PosicaoXadrez('d', 8).ToPosicao());



        }

        public void RealizarJogada(Posicao origem, Posicao destino)
        {
            ExecutarMovimento(origem, destino);
            Turno++;
            MudarJogador();
        }
        public void ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaMovimentada = Tabuleiro.RetirarPeca(origem);
            pecaMovimentada.IncrementarQtdMovimento();
            Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
            Tabuleiro.ColocarPeca(pecaMovimentada, destino);

        }
        public void ValidarPosicaoOrigem(Posicao posicao)
        {
            if (Tabuleiro.GetPeca(posicao) == null)
                throw new TabuleiroException("Não existe peça na posição escolhida!");
            if (JogadorAtual != Tabuleiro.GetPeca(posicao).Cor)
                throw new TabuleiroException("A peça escolhida não é sua!");
            if (!Tabuleiro.GetPeca(posicao).ExisteMovimentoPossiveis())
                throw new TabuleiroException("Não há movimentos para esta peça!");
        }
        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tabuleiro.GetPeca(origem).PodeRealizarMovimento(destino))
                throw new TabuleiroException("A posição de destino não é válida");
        }

        public void MudarJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

    }


}
