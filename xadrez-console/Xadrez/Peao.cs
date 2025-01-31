﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;

namespace Xadrez
{
    internal class Peao : Peca
    {
        private PartidaDeXadrez partida;

        public Peao(Cor cor, Tabuleiro tab, PartidaDeXadrez partida) : base(cor, tab)
        {
            this.partida = partida;
        }

        private bool ExisteInimigo(Posicao pos)
        {
            Peca p = Tab.ObterPeca(pos);
            return p != null && p.Cor != Cor;
        }

        private bool Livre(Posicao pos)
        {
            Peca p = Tab.ObterPeca(pos);
            return p == null;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            if (Cor == Cor.Azul)
            {
                //cima
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                    pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos) && QteDeMovimentos == 0)
                        mat[pos.Linha, pos.Coluna] = true;
                }

                //no
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //ne
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //#jogadaespecial en passant
                if (Posicao.Linha == 3)
                {
                    Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.ObterPeca(esquerda) == partida.VulneravelEnPassant)
                        mat[esquerda.Linha - 1, esquerda.Coluna] = true;

                    Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.ObterPeca(direita) == partida.VulneravelEnPassant)
                        mat[direita.Linha - 1, direita.Coluna] = true;
                }
            }
            else
            {
                //cima
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                    pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos) && QteDeMovimentos == 0)
                        mat[pos.Linha, pos.Coluna] = true;
                }

                //no
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //ne
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //#jogadaespecial en passant
                if (Posicao.Linha == 4)
                {
                    Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.ObterPeca(esquerda) == partida.VulneravelEnPassant)
                        mat[esquerda.Linha + 1, esquerda.Coluna] = true;

                    Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.ObterPeca(direita) == partida.VulneravelEnPassant)
                        mat[direita.Linha + 1, direita.Coluna] = true;
                }
            }
            return mat;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}
