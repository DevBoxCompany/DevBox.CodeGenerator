using System.Windows.Forms;

namespace DevBox.CodeGenerator
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
        }

        public void Warn(string text)
        {
            MessageBox.Show(text, @"Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void Error(string text)
        {
            MessageBox.Show(text, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
