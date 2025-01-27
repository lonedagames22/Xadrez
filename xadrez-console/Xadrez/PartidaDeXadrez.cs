using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;

namespace Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool PartidaTerminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Azul;
            PartidaTerminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            InicializarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteDeMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
                capturadas.Add(pecaCapturada);
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in capturadas)
            {
                if (p.Cor == cor)
                    aux.Add(p);
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> pecasEmJogo = new HashSet<Peca>(pecas.Where(p => p.Cor == cor));
            pecasEmJogo.ExceptWith(PecasCapturadas(cor));
            return pecasEmJogo;
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            Peca peca = Tab.ObterPeca(pos);
            if (peca == null)
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            if (JogadorAtual != peca.Cor)
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            if (!peca.ExisteMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            Peca peca = Tab.ObterPeca(origem);
            if (!peca.PodeMoverPara(destino))
                throw new TabuleiroException("Posição de destino inválida!");
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Vermelha)
                JogadorAtual = Cor.Azul;
            else
                JogadorAtual = Cor.Vermelha;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void InicializarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('c', 2, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('d', 2, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('e', 2, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('e', 1, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('d', 1, new Rei(Cor.Azul, Tab));

            ColocarNovaPeca('c', 7, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('c', 8, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('d', 8, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('e', 8, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('e', 7, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('d', 7, new Rei(Cor.Vermelha, Tab));

        }

    }
}
