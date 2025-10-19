using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CarDealerApp.Views
{
    public partial class ClientEditWindow : Window
    {
        public ClientEditWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void TipoPessoaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DocumentoTextBox != null)
            {
                DocumentoTextBox.Clear();
            }
        }

        private void DocumentoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            textBox.TextChanged -= DocumentoTextBox_TextChanged;

            string digitsOnly = new string(textBox.Text.Where(char.IsDigit).ToArray());
            string formattedText = digitsOnly;

            string tipoPessoa = TipoPessoaComboBox.SelectedItem as string;

            if (tipoPessoa == "fisica") // Formato CPF: ###.###.###-##
            {
                if (digitsOnly.Length > 11) digitsOnly = digitsOnly.Substring(0, 11);

                if (digitsOnly.Length > 9)
                    formattedText = string.Format("{0}.{1}.{2}-{3}", digitsOnly.Substring(0, 3), digitsOnly.Substring(3, 3), digitsOnly.Substring(6, 3), digitsOnly.Substring(9));
                else if (digitsOnly.Length > 6)
                    formattedText = string.Format("{0}.{1}.{2}", digitsOnly.Substring(0, 3), digitsOnly.Substring(3, 3), digitsOnly.Substring(6));
                else if (digitsOnly.Length > 3)
                    formattedText = string.Format("{0}.{1}", digitsOnly.Substring(0, 3), digitsOnly.Substring(3));
            }
            else if (tipoPessoa == "juridica") // Formato CNPJ: ##.###.###/####-##
            {
                if (digitsOnly.Length > 14) digitsOnly = digitsOnly.Substring(0, 14);

                if (digitsOnly.Length > 12)
                    formattedText = string.Format("{0}.{1}.{2}/{3}-{4}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2, 3), digitsOnly.Substring(5, 3), digitsOnly.Substring(8, 4), digitsOnly.Substring(12));
                else if (digitsOnly.Length > 8)
                    formattedText = string.Format("{0}.{1}.{2}/{3}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2, 3), digitsOnly.Substring(5, 3), digitsOnly.Substring(8));
                else if (digitsOnly.Length > 5)
                    formattedText = string.Format("{0}.{1}.{2}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2, 3), digitsOnly.Substring(5));
                else if (digitsOnly.Length > 2)
                    formattedText = string.Format("{0}.{1}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2));
            }

            textBox.Text = formattedText;
            textBox.CaretIndex = formattedText.Length;

            textBox.TextChanged += DocumentoTextBox_TextChanged;
        }

        private void TelefoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            textBox.TextChanged -= TelefoneTextBox_TextChanged;

            string digitsOnly = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length > 11)
            {
                digitsOnly = digitsOnly.Substring(0, 11);
            }

            string formattedText = digitsOnly;

            if (digitsOnly.Length > 0)
            {
                if (digitsOnly.Length > 6)
                    formattedText = string.Format("({0}) {1}-{2}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2, 5), digitsOnly.Substring(7));
                else if (digitsOnly.Length > 2)
                    formattedText = string.Format("({0}) {1}", digitsOnly.Substring(0, 2), digitsOnly.Substring(2));
                else
                    formattedText = string.Format("({0}", digitsOnly);
            }

            textBox.Text = formattedText;
            textBox.CaretIndex = formattedText.Length;

            textBox.TextChanged += TelefoneTextBox_TextChanged;
        }
    }
}