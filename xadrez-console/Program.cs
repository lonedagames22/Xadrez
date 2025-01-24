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
            PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            
            Console.WriteLine(pos);
            Console.WriteLine(pos.ToPosicao());
        }
    }
}