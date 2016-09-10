using DevBox.CodeGenerator.Core.Gerador;
using DevBox.CodeGenerator.Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevBox.CodeGenerator.Core.Models;

namespace DevBox.CodeGenerator
{
    public partial class FormPrincipal : FormBase
    {
        private List<CampoTabela> _camposTabela;

        public FormPrincipal()
        {
            InitializeComponent();

            cbConnections.ValueMember = "ConnectionString";
            cbConnections.DisplayMember = "Name";
            cbConnections.DataSource = new Connections().Get();
        }

        private void btnBuscarFields_Click(object sender, EventArgs e)
        {
            var nomeTabela = txtNomeTabela.Text;
            var connection = cbConnections.SelectedValue.ToString();

            if (string.IsNullOrEmpty(nomeTabela))
            {
                Warn(@"Informe a tabela.");
                return;
            }

            try
            {
                _camposTabela = new TabelaRepository(connection)
                    .BuscarCamposPorNomeTabela(txtNomeTabela.Text);

                var componentes = new Componentes();

                foreach (var item in _camposTabela)
                    panel1.Controls.Add(componentes.Create(item, this));
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        public void alias_KeyUp(object sender, KeyEventArgs e)
        {
            var a = (sender as TextBox);

            if (a == null)
                return;

            var fieldId = int.Parse(a.Parent.Tag.ToString());

            var obj = _camposTabela.First(x => x.Id == fieldId);

            obj.Alias = a.Text == string.Empty ? null : a.Text;
        }

        public void methods_CheckedChanged(object sender, EventArgs e)
        {
            var a = (sender as CheckBox);

            if (a == null)
                return;

            var fieldId = int.Parse(a.Parent.Tag.ToString());

            var obj = _camposTabela.First(x => x.Id == fieldId);

            switch (int.Parse(a.Tag.ToString()))
            {
                case (int)Method.Post:
                    obj.Post = a.Checked;
                    break;
                case (int)Method.Put:
                    obj.Put = a.Checked;
                    break;
                case (int)Method.Delete:
                    obj.Delete = a.Checked;
                    break;
                case (int)Method.Get:
                    obj.Get = a.Checked;
                    break;
                case (int)Method.GetId:
                    obj.GetId = a.Checked;
                    break;
                default:
                    break;
            }
        }

        private void methodsSelect_CheckedChanged(object sender, EventArgs e)
        {
            var a = (sender as CheckBox);

            if (a == null)
                return;

            var fieldId = int.Parse(a.Tag.ToString());

            foreach (Control pn1 in panel1.Controls)
                if (pn1.GetType() == typeof(Panel))
                    foreach (var pn2 in pn1.Controls)
                        if (pn2.GetType() == typeof(CheckBox))
                        {
                            var b = ((CheckBox)pn2);
                            if (int.Parse(b.Tag.ToString()) == fieldId)
                                b.Checked = a.Checked;
                        }

        }

        private void selectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                lblDiretorio.Text = folderBrowserDialog1.SelectedPath;

                btnBucarDiretorio.Left = lblDiretorio.Left + lblDiretorio.Width + 2;
            }
        }

        private void btnGerarProcedures_Click(object sender, EventArgs e)
        {
            var fileName = txtFile.Text;
            var directory = lblDiretorio.Text;

            if (string.IsNullOrEmpty(fileName))
                return;
            if (string.IsNullOrEmpty(directory))
                return;

            var procedure = new Procedures(new Tabela(txtNomeTabela.Text)
            {
                CamposTabela = _camposTabela
            });

            using (var file = new StreamWriter($@"{directory}\{fileName}.sql", true))
            {
                file.Write(procedure.GerarCompleto());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cbConnections.SelectedValue.ToString());
        }
    }
}
