﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;

namespace Xadrez
{
    internal class Rei : Peca
    {
        public Rei(Cor cor, Tabuleiro tab) : base(cor, tab)
        {
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.ObterPeca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //ne
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //direita
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //ne
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //abaixo
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //so
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //esquerda
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            //no
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;
            return mat;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
