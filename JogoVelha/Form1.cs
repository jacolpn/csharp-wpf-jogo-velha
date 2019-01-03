using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoVelha
{
    public partial class Form1 : Form
    {
        string[] textos = new string[9];
        int rodadas = 0;
        bool turno = true, fimDeJogo = false, IAJogou = false;
        Random rdn = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        void ChecarVencedor()
        {
            // Horizontais:
            string Vazio = turno ? "X" : "O";

            for (int i = 0; i <= 6; i += 3)
            {
                if (textos[i] == Vazio && textos[i] == textos[i +1] && textos[i] == textos[i +2])
                {
                    Vencedor();
                }
            }

            // Verticais:
            for (int i = 0; i <= 2; i++)
            {
                if (textos[i] == Vazio && textos[i] == textos[i +3] && textos[i] == textos[i + 6])
                {
                    Vencedor();
                }
            }
            
            // Diagonais:
                //Diagonal Principal:
            if (textos[0] == Vazio && textos[0] == textos[4] && textos[0] == textos[8])
            {
                Vencedor();
            }

                //Diagonal Secundária:
            if (textos[2] == Vazio && textos[2] == textos[4] && textos[2] == textos[6])
            {
                Vencedor();
            }

            // Empate:
            if (rodadas == 9 && !fimDeJogo)
            {
                fimDeJogo = true;
                MessageBox.Show("Empate!");
            }
        }
        void Vencedor()
        {
            fimDeJogo = true;
            MessageBox.Show(String.Format("Jogador {0} venceu!", turno ? "X" : "O"));
        }
        private void Botoes(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (turno && !fimDeJogo && btn.Text == "")
            {
                btn.Text = "X";
                textos[btn.TabIndex] = btn.Text;
                rodadas++;
                ChecarVencedor();
                turno = !turno;
                PensarIA();
            }
        }
        private void Random()
        {
            bool encontrouLocal = false;

            if (!turno)
                while (!encontrouLocal)
                {
                    int sortear = rdn.Next(0, 9);

                    foreach (var bt in Controls.OfType<Button>())
                    {
                        if (bt.Text == "" && bt.TabIndex == sortear)
                        {
                            // Encontrou um local válido para marcar no botão.
                            encontrouLocal = true;
                            IAJogar(sortear);
                        }
                    }
                }
        }
        private void IAJogar (int tabindex)
        {
            foreach (var bt in Controls.OfType<Button>())
            {
                if (bt.TabIndex == tabindex && !turno && bt.Text == "")
                {
                    bt.Text = "O";
                    textos[bt.TabIndex] = bt.Text;
                    rodadas++;
                    IAJogou = true;
                    ChecarVencedor();
                    turno = !turno;
                }
            }
        }
        private void IAValidar(string jogador)
        {
            // Checar horizontais:
            // Variáveis locais de auxílio.
            int iniciar = 0, limite = 0, verificarAoLado = 0, argumento = 0;

            if (!turno && !fimDeJogo)
            {
                // Laço que representa as 3 formas de vencer
                for (int i = 1; i <= 3; i++)
                {
                    // Validar o laço para modificar as variáveis auxiliares.
                    switch (i)
                    {
                        case 1:
                            limite = 6; verificarAoLado = 1; argumento = 2;
                            break;
                        case 2:
                            verificarAoLado = 2; argumento = 1;
                            break;
                        case 3:
                            iniciar = 1; limite = 7; verificarAoLado = 1; argumento = -1;
                            break;
                    }

                    // Checar todas as 3 linhas das horizontais.
                    for (int j = iniciar; j <= limite; j += 3)
                    {
                        if (textos[j] == jogador && textos[j] == textos[j + verificarAoLado])
                        {
                            /* Essa validação verifica se o conteúdo do vetor na posição do incremento do loop é igual ao parâmetro,
                             * e se é igual ao do botão ao seu lado, e se o botão que deseja marcar está vazio, fazendo com que
                             * o método para marcar a bolinha(o) seja chamado, passando como argumento o botão desejado. */
                            IAJogar(j + argumento);
                        }
                    }
                }
            }

            // Checar verticais:
            for (int i = 1; i <= 3; i++)
            {
                switch (i)
                {
                    case 1:
                        iniciar = 0; limite = 2; verificarAoLado = 3; argumento = 6;
                        break;
                    case 2:
                        verificarAoLado = 6; argumento = 3;
                        break;
                    case 3:
                        iniciar = 3; limite = 5; verificarAoLado = 3; argumento = -3;
                        break;
                }

                for (int j = iniciar; j <= limite; j++)
                {
                    if (textos[j] == jogador && textos[j] == textos[j + verificarAoLado])
                    {
                        IAJogar(j + argumento);
                    }
                }
            }

            //Checar diagonais:
            for (int i = 1; i <= 6; i++)
            {
                switch (i)
                {
                    case 1:
                        iniciar = 0; verificarAoLado = 4; argumento = 8;
                        break;
                    case 2:
                        verificarAoLado = 8; argumento = 4;
                        break;
                    case 3:
                        iniciar = 4; verificarAoLado = 8; argumento = 0;
                        break;
                    case 4:
                        iniciar = 2; verificarAoLado = 4; argumento = 6;
                        break;
                    case 5:
                        verificarAoLado = 6; argumento = 4;
                        break;
                    case 6:
                        iniciar = 4; verificarAoLado = 6; argumento = 2;
                        break;
                }

                if (textos[iniciar] == jogador && textos[iniciar] == textos[verificarAoLado])
                {
                    IAJogar(argumento);
                }
            }
        }
        private void PensarIA()
        {
            IAJogou = false;

            for (int i = 0; i <= 2; i++)
            {
                if (IAJogou)
                {
                    break;
                }

                if (!IAJogou && !fimDeJogo)
                {
                    switch (i)
                    {
                        //Vai tentar atacar.
                        case 0:
                            IAValidar("O");
                            break;
                        //Vai tentar defender.
                        case 1:
                            IAValidar("X");
                            break;
                        //Vai jogar aleatório.
                        case 2:
                            Random();
                            break;
                    }
                }
            }
        }
        private void BotaoLimpar(object sender, EventArgs e)
        {
            rodadas = 0;
            fimDeJogo = false;
            turno = !turno;
            IAJogou = false;

            foreach (var buttons in Controls.OfType<Button>())
            {
                buttons.Text = "";
            }
            for (int i = 0; i < textos.Length; i++)
            {
                textos[i] = "";
            }
            btnLimpar.Text = "Reiniciar";

            if (!turno)
            {
                PensarIA();
            }
        }
    }
}
