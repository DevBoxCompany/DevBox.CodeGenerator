using System.Collections.Generic;
using System.Windows.Forms;
using DevBox.CodeGenerator.Core.Models;

namespace DevBox.CodeGenerator
{
    public class Componentes
    {
        public int Y { get; set; }
        public Componentes()
        {
            Y = 2;
        }

        public Label AttrName(CampoTabela field)
        {
            return new Label
            {
                AutoSize = false,
                Location = new System.Drawing.Point(17, 7),
                Name = "label" + field.Id,
                Size = new System.Drawing.Size(155, 13),
                Text = field.NomeColuna,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
        }
        public Label As()
        {
            return new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(178, 7),
                Size = new System.Drawing.Size(21, 13),
                Text = @"AS"
            };
        }
        public TextBox Alias(CampoTabela field)
        {
            return new TextBox()
            {
                Location = new System.Drawing.Point(215, 3),
                Name = "textBox" + field.Id,
                Size = new System.Drawing.Size(100, 20),
                TabIndex = 1
            };
        }
        public CheckBox Check(CampoTabela field, int i)
        {
            return new CheckBox()
            {
                AutoSize = true,
                Location = new System.Drawing.Point(335 + (i * 40), 7),
                Name = "checkBox" + field.Id + i,
                Size = new System.Drawing.Size(15, 14),
                UseVisualStyleBackColor = true,
                Checked = !field.AutoIncremento,
                Tag = i + 1,
            };
        }

        public Panel Create(CampoTabela field, FormPrincipal form)
        {
            var panel = new Panel
            {
                Location = new System.Drawing.Point(3, Y),
                Name = "panel" + field.Id,
                Size = new System.Drawing.Size(569, 27),
                Tag = field.Id,
                BorderStyle = BorderStyle.Fixed3D
            };

            #region Objs

            var lb1 = AttrName(field);
            var lb2 = As();
            var txt = Alias(field);
            var chkList = new CheckBox[5];
            for (var i = 0; i < 5; i++)
                chkList[i] = Check(field, i);

            #endregion

            panel.Controls.Add(lb1);
            panel.Controls.Add(lb2);
            panel.Controls.Add(txt);

            txt.KeyUp += form.alias_KeyUp;
            for (var i = 0; i < 5; i++)
            {
                panel.Controls.Add(chkList[i]);
                chkList[i].CheckedChanged += form.methods_CheckedChanged;
            }

            Y += 29;
            return panel;
        }
    }

    public enum Method
    {
        Post = 1,
        Put = 2,
        Delete = 5,
        Get = 3,
        GetId = 4
    }

    public class Connections
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }

        public List<Connections> Get()
        {
            var a = new List<Connections>
            {
                new Connections
                {
                    Name = "Loteria Federal",
                    ConnectionString =
                        @"Data Source=.\SQLEXPRESS;Initial Catalog=LoteriaFederal12;Integrated Security=True;Pooling=False;Uid=auth_window"
                },
                new Connections
                {
                    Name = "Banco Teste",
                    ConnectionString =
                        @"Server=MUSSAK-PC\SQLEXPRESS;Database=BancoTeste;Trusted_Connection=True;MultipleActiveResultSets=true"
                }
            };

            return a;
        }
    }
}

