using System;
using tabuleiro;

namespace xadrez_console
{
    class Program
    {
        static void Main()
        {
            Tabuleiro tab = new Tabuleiro(8, 8);

            Tela.ImprimirTabuleiro(tab);
        }
    }
}
