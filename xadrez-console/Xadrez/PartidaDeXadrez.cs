using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jogo.Tabuleiro;
using xadrez_console;

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
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Azul;
            PartidaTerminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            Xeque = false;
            VulneravelEnPassant = null;
            InicializarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteDeMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
                capturadas.Add(pecaCapturada);

            //#jogadaespecial roque pequeno 
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteDeMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            //#jogadaespecial roque grande 
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteDeMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            //#jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Azul)
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    else
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    pecaCapturada = Tab.RetirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
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

        private Cor CorAdversaria(Cor cor)
        {
            if (cor == Cor.Azul)
                return Cor.Vermelha;
            return Cor.Azul;
        }

        private Peca ObterRei(Cor cor)
        {
            foreach (Peca p in PecasEmJogo(cor))
            {
                if (p is Rei)
                    return p;
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca rei = ObterRei(cor);
            foreach (Peca p in PecasEmJogo(CorAdversaria(cor)))
            {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[rei.Posicao.Linha, rei.Posicao.Coluna])
                    return true;
            }
            return false;
        }

        public bool TesteXequemate(Cor cor)
        {
            if (!EstaEmXeque(cor))
                return false;
            foreach (Peca p in PecasEmJogo(cor))
            {
                bool[,] mat = p.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQteDeMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            //#jogadaespecial roque pequeno 
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQteDeMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            //#jogadaespecial roque grande 
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.IncrementarQteDeMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            //#jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Azul)
                        posP = new Posicao(3, destino.Coluna);
                    else
                        posP = new Posicao(4, destino.Coluna);
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = Tab.ObterPeca(destino);
            
            //#jogadaespecial promoção
            if(p is Peao) 
            {
                if((p.Cor == Cor.Azul && destino.Linha == 0) || (p.Cor == Cor.Vermelha && destino.Linha == 7)) 
                {
                    p = Tab.RetirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(p.Cor, Tab);
                    Tab.ColocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (EstaEmXeque(CorAdversaria(JogadorAtual)))
                Xeque = true;
            else
                Xeque = false;

            if (TesteXequemate(CorAdversaria(JogadorAtual)))
                PartidaTerminada = true;
            else
            {
                Turno++;
                MudaJogador();
            }

            //#jogadaespecial en passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
                VulneravelEnPassant = p;
            else
                VulneravelEnPassant = null;
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
            if (!peca.MovimentoPossivel(destino))
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
            ColocarNovaPeca('a', 1, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Azul, Tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Azul, Tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Azul, Tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Azul, Tab, this));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Azul, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Azul, Tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Azul, Tab));
            ColocarNovaPeca('a', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Azul, Tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Azul, Tab, this));

            ColocarNovaPeca('a', 8, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Vermelha, Tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Vermelha, Tab));
            ColocarNovaPeca('d', 8, new Dama(Cor.Vermelha, Tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Vermelha, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Vermelha, Tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Vermelha, Tab));
            ColocarNovaPeca('a', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Vermelha, Tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Vermelha, Tab, this));
        }
    }
}
