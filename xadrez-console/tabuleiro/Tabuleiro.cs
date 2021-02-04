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

        public Peca GetPeca(int linha,int coluna)
        {
            return Pecas[linha, coluna];
        }  
       /* public void AdicionarPeca(int linha, int coluna, Peca peca)
        {
            Pecas[linha, coluna] = peca;
        }*/
    }
}
