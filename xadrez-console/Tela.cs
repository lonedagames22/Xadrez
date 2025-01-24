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
                Console.Write(8 - i + " ");
                for (int j = 0; j < 8; j++)
                {
                    if (tab.ObterPeca(i, j) == null)
                        Console.Write("- ");
                    else
                    {
                        ImprimirPeca(tab.ObterPeca(i, j));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirPeca(Peca peca)
        {
            ConsoleColor aux = Console.ForegroundColor;
            if (peca.Cor == Cor.Vermelha)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(peca);
            }
            else if (peca.Cor == Cor.Azul)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(peca);
            }
            Console.ForegroundColor = aux;
        }
    }
}
