using DevBox.CodeGenerator.Core.Gerador;
using DevBox.CodeGenerator.Core.Repository;
using System;
using System.IO;
using System.Windows.Forms;

namespace DevBox.CodeGenerator
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnBuscarFields_Click(object sender, EventArgs e)
        {
            var nomeTabela = txtNomeTabela.Text;
            if (string.IsNullOrEmpty(nomeTabela))
                return;

            var lstCamposTabela = new TabelaRepository().BuscarCamposPorNomeTabela(nomeTabela);

            var procedure = new Procedures(new Core.Models.Tabela(nomeTabela)
            {
                CamposTabela = lstCamposTabela
            });

            using (var file = new StreamWriter($@"C:\Users\Vinícius\Desktop\Gerador\{nomeTabela}.sql", true))
            {
                file.Write(procedure.GerarCompleto());
            }

        }

    }
}
