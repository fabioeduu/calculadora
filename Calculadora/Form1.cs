using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;

namespace Calculadora
{
    public partial class Calculadora : Form
    {
        public Calculadora()
        {
            InitializeComponent();
            AssociarEventos(this.Controls);
        }

        private void AssociarEventos(Control.ControlCollection controles)
        {
            foreach (Control ctrl in controles)
            {
                if (ctrl is Button btn)
                {
                    if ("0123456789".Contains(btn.Text))
                        btn.Click += Numero_Click;
                    else if ("+-*/×÷−".Contains(btn.Text))
                        btn.Click += Operador_Click;
                    else if (btn.Text == "Xʸ" || btn.Text == "xʸ" || btn.Text == "^")
                        btn.Click += Potencia_Click;
                    else if (btn.Text == "√y" || btn.Text == "√")
                        btn.Click += Raiz_Click;
                    else if (btn == igual)
                        btn.Click += Igual_Click;
                    else if (btn == clean)
                        btn.Click += Clean_Click;
                    else if (btn == Virgula)
                        btn.Click += Virgula_Click;
                }
                if (ctrl.HasChildren)
                    AssociarEventos(ctrl.Controls);
            }
        }

        private void Numero_Click(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            Resultado.Text += botao.Text;
        }

        private void Virgula_Click(object sender, EventArgs e)
        {
            if (Resultado.Text.Length == 0 || "+-*/×÷−".Contains(Resultado.Text[Resultado.Text.Length - 1].ToString()))
            {
                Resultado.Text += "0,";
            }
            else if (!Resultado.Text.EndsWith(","))
            {
                Resultado.Text += ",";
            }
        }

        private void Operador_Click(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            string operadorAtual = botao.Text;

            if (Resultado.Text.Length > 0 && !"+-*/×÷−".Contains(Resultado.Text[Resultado.Text.Length - 1].ToString()))
            {
                Resultado.Text += operadorAtual;
            }
        }

        private void Potencia_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = Resultado.Text.Replace(',', '.');
                double valor;
                if (double.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                {
                    double resultado = Math.Pow(valor, 2);
                    Resultado.Text = resultado.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    Resultado.Text = "Erro";
                }
            }
            catch
            {
                Resultado.Text = "Erro";
            }
        }

        private void Raiz_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = Resultado.Text.Replace(',', '.');
                double valor;
                if (double.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                {
                    if (valor < 0)
                    {
                        Resultado.Text = "Erro";
                    }
                    else
                    {
                        double resultado = Math.Sqrt(valor);
                        Resultado.Text = resultado.ToString(CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    Resultado.Text = "Erro";
                }
            }
            catch
            {
                Resultado.Text = "Erro";
            }
        }

        private void Igual_Click(object sender, EventArgs e)
        {
            try
            {
                string expressao = Resultado.Text.Replace(',', '.');
                expressao = expressao.Replace("×", "*").Replace("÷", "/").Replace("−", "-");
                while (expressao.Length > 0 && "+-*/".Contains(expressao[expressao.Length - 1]))
                    expressao = expressao.Substring(0, expressao.Length - 1);
                int openPar = expressao.Count(c => c == '(');
                int closePar = expressao.Count(c => c == ')');
                for (int i = 0; i < openPar - closePar; i++)
                    expressao += ")";
                var resultado = new DataTable().Compute(expressao, null);
                Resultado.Text = Convert.ToDouble(resultado).ToString(CultureInfo.CurrentCulture);
            }
            catch
            {
                Resultado.Text = "Erro";
            }
        }

        private void Clean_Click(object sender, EventArgs e)
        {
            Resultado.Text = "";
        }

        private void Calculadora_Load(object sender, EventArgs e)
        {
        }
    }
}
