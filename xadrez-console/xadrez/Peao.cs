
using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrez Partida;
        public Peao(Tabuleiro tabuleiro, Cor cor, PartidaDeXadrez partida) : base(tabuleiro, cor)
        {
            Partida = partida;
        }

        public bool ExisteInimigo(Posicao posicao)
        {
            Peca peca = Tabuleiro.GetPeca(posicao);
            return peca != null && peca.Cor != Cor;
        }

        public bool EstaLivre(Posicao posicao)
        {
            return Tabuleiro.GetPeca(posicao) == null;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao posicao = new Posicao(0, 0);
            Posicao posicaoAux = new Posicao(0, 0);

            if (Cor == Cor.Branca)
            {
                posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tabuleiro.PosicaoValida(posicao) && EstaLivre(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;

                posicaoAux.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                if (Tabuleiro.PosicaoValida(posicaoAux) && EstaLivre(posicaoAux) && EstaLivre(posicao) && QtdMovimentos == 0)
                    matriz[posicaoAux.Linha, posicaoAux.Coluna] = true;

                posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tabuleiro.PosicaoValida(posicao) && ExisteInimigo(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;

                posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tabuleiro.PosicaoValida(posicao) && ExisteInimigo(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;
                //jogada especial en passant
                if (Posicao.Linha == 3)
                {
                    Posicao posEsquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tabuleiro.PosicaoValida(posEsquerda) && ExisteInimigo(posEsquerda) && Tabuleiro.GetPeca(posEsquerda) == Partida.VuneravelEnPassant)
                    {
                        matriz[posEsquerda.Linha - 1, posEsquerda.Coluna] = true;
                    }
                    Posicao posDireita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tabuleiro.PosicaoValida(posDireita) && ExisteInimigo(posDireita) && Tabuleiro.GetPeca(posDireita) == Partida.VuneravelEnPassant)
                    {
                        matriz[posDireita.Linha - 1, posDireita.Coluna] = true;
                    }
                }

            }
            else
            {
                posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tabuleiro.PosicaoValida(posicao) && EstaLivre(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;

                posicaoAux.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                if (Tabuleiro.PosicaoValida(posicaoAux) && EstaLivre(posicaoAux) && EstaLivre(posicao) && QtdMovimentos == 0)
                    matriz[posicaoAux.Linha, posicaoAux.Coluna] = true;

                posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tabuleiro.PosicaoValida(posicao) && ExisteInimigo(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;

                posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tabuleiro.PosicaoValida(posicao) && ExisteInimigo(posicao))
                    matriz[posicao.Linha, posicao.Coluna] = true;

                //jogada especial en passant
                if (Posicao.Linha == 4)
                {
                    Posicao posEsquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tabuleiro.PosicaoValida(posEsquerda) && ExisteInimigo(posEsquerda) && Tabuleiro.GetPeca(posEsquerda) == Partida.VuneravelEnPassant)
                    {
                        matriz[posEsquerda.Linha + 1, posEsquerda.Coluna] = true;
                    }
                    Posicao posDireita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tabuleiro.PosicaoValida(posDireita) && ExisteInimigo(posDireita) && Tabuleiro.GetPeca(posDireita) == Partida.VuneravelEnPassant)
                    {
                        matriz[posDireita.Linha + 1, posDireita.Coluna] = true;
                    }
                }

            }


            return matriz;
        }

        public override string ToString()
        {
            return "P";
        }


    }
}
