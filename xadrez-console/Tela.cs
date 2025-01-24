using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;
using Xadrez;

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
                    ImprimirPeca(tab.ObterPeca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < 8; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < 8; j++)
                {
                    if (posicoesPossiveis[i, j])
                        Console.BackgroundColor = fundoAlterado;
                    else
                        Console.BackgroundColor = fundoOriginal;
                    ImprimirPeca(tab.ObterPeca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca(Peca peca)
        {

            if (peca == null)
                Console.Write("- ");
            else
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
                Console.Write(" ");
            }
        }
    }
}
