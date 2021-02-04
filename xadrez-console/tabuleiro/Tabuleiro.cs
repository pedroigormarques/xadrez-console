using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }

        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            Pecas = new Peca[linhas, colunas];
        }

        public Peca GetPeca(int linha, int coluna)
        {
            return Pecas[linha, coluna];
        }

        public Peca GetPeca(Posicao posicao)
        {
            return Pecas[posicao.Linha, posicao.Coluna];
        }
        public void ColocarPeca(Peca peca, Posicao posicao)
        {
            if (ExistePeca(posicao))
                throw new TabuleiroException("Já existe uma peça nessa posição!");
           
            Pecas[posicao.Linha, posicao.Coluna] = peca;
            Pecas[posicao.Linha, posicao.Coluna].Posicao = posicao;
        }

        public Peca RetirarPeca(Posicao posicao)
        {
            if (GetPeca(posicao) == null)
                return null;
            Peca aux = GetPeca(posicao);
            aux.Posicao = null;
            Pecas[posicao.Linha, posicao.Coluna] = null;
            return aux;
        }

        public bool PosicaoValida(Posicao posicao)
        {
            if (posicao.Coluna < 0 || posicao.Coluna >= Colunas || posicao.Linha < 0 || posicao.Linha >= Linhas)
                return false;
            else
                return true;
        }

        public void ValidarPosicao(Posicao posicao)
        {
            if (!PosicaoValida(posicao))
                throw new TabuleiroException("Posicao inválida!");
        }

        public bool ExistePeca(Posicao posicao)
        {
            ValidarPosicao(posicao);
            if (GetPeca(posicao) == null)
                return false;
            else
                return true;
        }

    }
}
