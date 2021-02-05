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
        public bool Xeque { get; private set; }
        public Peca VuneravelEnPassant { get; private set; }

        private HashSet<Peca> Pecas;
        private HashSet<Peca> PecasCapturadas;

        public PartidaDeXadrez()
        {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VuneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tabuleiro, Cor.Branca));
            //ColocarNovaPeca('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tabuleiro, Cor.Branca));
            /*for (int i = 97; i < 105; i++)
                ColocarNovaPeca((char)i, 2, new Peao(Tabuleiro, Cor.Branca, this));*/

            ColocarNovaPeca('a', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tabuleiro, Cor.Preta));
            for (int i = 97; i < 105; i++)
                ColocarNovaPeca((char)i, 7, new Peao(Tabuleiro, Cor.Preta, this));
        }
        public HashSet<Peca> PecasEmjogoPorCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca peca in Pecas)
            {
                if (peca.Cor == cor)
                    aux.Add(peca);
            }
            aux.ExceptWith(PecasCapturadasPorCor(cor));

            return aux;
        }

        public HashSet<Peca> PecasCapturadasPorCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca peca in PecasCapturadas)
            {
                if (peca.Cor == cor)
                    aux.Add(peca);
            }
            return aux;
        }

        public void RealizarJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazerMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca pecaMovida = Tabuleiro.GetPeca(destino);
            //jogada especial promoção
            if (pecaMovida is Peao)
            {
                if ((pecaMovida.Cor == Cor.Branca && destino.Linha == 0) || (pecaMovida.Cor == Cor.Preta && destino.Linha == 7))
                {
                    pecaMovida = Tabuleiro.RetirarPeca(destino);
                    Pecas.Remove(pecaMovida);
                    Peca dama = new Dama(Tabuleiro, pecaMovida.Cor);
                    Tabuleiro.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                }

                    

            }


            Xeque = EstaEmXeque(CorAdversária(JogadorAtual));

            if (TesteXequemate(CorAdversária(JogadorAtual)))
                Terminada = true;
            else
            {
                Turno++;
                MudarJogador();
            }

            //jogada especial en passant
            if (pecaMovida is Peao && (origem.Linha + 2 == destino.Linha || origem.Linha - 2 == destino.Linha))
            {
                VuneravelEnPassant = pecaMovida;
            }


        }
        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaMovimentada = Tabuleiro.RetirarPeca(origem);
            pecaMovimentada.IncrementarQtdMovimento();
            Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
            if (pecaCapturada != null)
                PecasCapturadas.Add(pecaCapturada);
            Tabuleiro.ColocarPeca(pecaMovimentada, destino);

            //Jogada especial Roque pequeno
            if (pecaMovimentada is Rei && origem.Coluna + 2 == destino.Coluna)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);

                Peca torreMovimentada = Tabuleiro.RetirarPeca(origemTorre);
                torreMovimentada.IncrementarQtdMovimento();
                Tabuleiro.ColocarPeca(torreMovimentada, destinoTorre);

            }
            //Jogada especial Roque grande
            if (pecaMovimentada is Rei && origem.Coluna - 2 == destino.Coluna)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca torreMovimentada = Tabuleiro.RetirarPeca(origemTorre);
                torreMovimentada.IncrementarQtdMovimento();
                Tabuleiro.ColocarPeca(torreMovimentada, destinoTorre);

            }

            //jogada especial en passant
            if (pecaMovimentada is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posicaoPeao;
                    if (pecaMovimentada.Cor == Cor.Branca)
                        posicaoPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                    else
                        posicaoPeao = new Posicao(destino.Linha - 1, destino.Coluna);
                    pecaCapturada = Tabuleiro.RetirarPeca(posicaoPeao);
                    PecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazerMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca pecaMovimentada = Tabuleiro.RetirarPeca(destino);
            pecaMovimentada.DecrementarQtdMovimento();
            if (pecaCapturada != null)
            {
                PecasCapturadas.Remove(pecaCapturada);
                Tabuleiro.ColocarPeca(pecaCapturada, destino);

            }
            Tabuleiro.ColocarPeca(pecaMovimentada, origem);

            //Jogada especial Roque pequeno
            if (pecaMovimentada is Rei && origem.Coluna + 2 == destino.Coluna)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);

                Peca torreMovimentada = Tabuleiro.RetirarPeca(destinoTorre);
                torreMovimentada.DecrementarQtdMovimento();
                Tabuleiro.ColocarPeca(torreMovimentada, origemTorre);

            }
            //Jogada especial Roque grande
            if (pecaMovimentada is Rei && origem.Coluna - 2 == destino.Coluna)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);

                Peca torreMovimentada = Tabuleiro.RetirarPeca(destinoTorre);
                torreMovimentada.DecrementarQtdMovimento();
                Tabuleiro.ColocarPeca(torreMovimentada, origemTorre);
            }
            //jogada especial en passant
            if (pecaMovimentada is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VuneravelEnPassant)
                {
                    Peca peao = Tabuleiro.RetirarPeca(destino);
                    Posicao posicaoPeao;
                    if (pecaMovimentada.Cor == Cor.Branca)
                        posicaoPeao = new Posicao(3, destino.Coluna);
                    else
                        posicaoPeao = new Posicao(4, destino.Coluna);
                    Tabuleiro.ColocarPeca(peao, posicaoPeao);
                }
            }
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
            if (!Tabuleiro.GetPeca(origem).MovimentoPossivel(destino))
                throw new TabuleiroException("A posição de destino não é válida");
        }

        public void MudarJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        private Cor CorAdversária(Cor cor)
        {
            if (cor == Cor.Branca)
                return Cor.Preta;
            else
                return Cor.Branca;
        }
        private Peca PecaRei(Cor cor)
        {
            foreach (Peca peca in PecasEmjogoPorCor(cor))
            {
                if (peca is Rei)
                    return peca;
            }
            return null;
        }
        public bool EstaEmXeque(Cor cor)
        {
            bool[,] matriz;
            Peca rei = PecaRei(cor);
            if (rei == null)
                throw new TabuleiroException("Não existe rei da cor " + cor + "no tabuleiro!");
            foreach (Peca peca in PecasEmjogoPorCor(CorAdversária(cor)))
            {
                matriz = peca.MovimentosPossiveis();
                if (matriz[rei.Posicao.Linha, rei.Posicao.Coluna])
                    return true;
            }
            return false;
        }
        public bool TesteXequemate(Cor cor)
        {
            if (!EstaEmXeque(cor))
                return false;
            foreach (Peca peca in PecasEmjogoPorCor(cor))
            {
                bool[,] matriz = peca.MovimentosPossiveis();
                for (int i = 0; i < Tabuleiro.Linhas; i++)
                {
                    for (int j = 0; j < Tabuleiro.Colunas; j++)
                    {
                        if (matriz[i, j])
                        {
                            Posicao origem = peca.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazerMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
    }


}
