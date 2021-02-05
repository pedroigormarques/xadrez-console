
using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        private PartidaDeXadrez Partida;
        public Rei(Tabuleiro tabuleiro, Cor cor, PartidaDeXadrez partida) : base(tabuleiro, cor)
        {
            Partida = partida;
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

            //Jogada Especial: Roque

            if (QtdMovimentos == 0 && !Partida.Xeque)
            {
                //roque pequeno
                Posicao posicaoT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                if (TesteTorreParaRoque(posicaoT1))
                {
                    Posicao posicaoP1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao posicaoP2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    if (Tabuleiro.GetPeca(posicaoP1) == null && Tabuleiro.GetPeca(posicaoP2) == null)
                        matriz[Posicao.Linha, Posicao.Coluna + 2] = true;
                }
                //Roque grande
                Posicao posicaoT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteTorreParaRoque(posicaoT1))
                {
                    Posicao posicaoP1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao posicaoP2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao posicaoP3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                    if (Tabuleiro.GetPeca(posicaoP1) == null && Tabuleiro.GetPeca(posicaoP2) == null && Tabuleiro.GetPeca(posicaoP3) == null)
                        matriz[Posicao.Linha, Posicao.Coluna - 2] = true;
                }
            }

            return matriz;
        }

        private bool TesteTorreParaRoque(Posicao posicao)
        {
            Peca peca = Tabuleiro.GetPeca(posicao);
            return peca != null && peca is Torre && peca.Cor == Cor && peca.QtdMovimentos == 0;
        }

        public override string ToString()
        {
            return "R";
        }

    }
}
