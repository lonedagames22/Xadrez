using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;

namespace xadrez_console
{
    internal class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tab.ObterPeca(i, j) == null) Console.Write("- ");
                    else Console.Write(tab.ObterPeca(i, j) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
