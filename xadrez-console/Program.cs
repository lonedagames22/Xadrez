using System;
using System.Data.Common;
using Jogo.Tabuleiro;
using Xadrez;

namespace xadrez_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(Cor.Vermelha, tab), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(Cor.Vermelha, tab), new Posicao(1, 3));
                tab.ColocarPeca(new Rei(Cor.Vermelha, tab), new Posicao(0, 2));
                tab.ColocarPeca(new Torre(Cor.Azul, tab), new Posicao(3, 5));

                Tela.ImprimirTabuleiro(tab);
                Console.WriteLine();
            }
            catch (TabuleiroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
        }
    }
}