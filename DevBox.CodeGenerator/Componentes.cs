﻿using DevBox.CodeGenerator.Core.Models;
using System.Collections.Generic;
using System.Windows.Forms;

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
            var chk = new CheckBox()
            {
                AutoSize = true,
                Location = new System.Drawing.Point(335 + (i * 40), 7),
                Name = "checkBox" + field.Id + (i + 1),
                Size = new System.Drawing.Size(15, 14),
                UseVisualStyleBackColor = true,
                Checked = !field.AutoIncremento,
                Tag = i + 1,
            };

            switch (i + 1)
            {
                case (int)Method.Post:
                    chk.Checked = !field.AutoIncremento;
                    break;
                case (int)Method.Put:
                    chk.Checked = !field.ChavePrimaria;
                    break;
                case (int)Method.Delete:
                    chk.Visible = field.ChavePrimaria;
                    chk.Checked = field.ChavePrimaria;
                    break;
                default:
                    chk.Checked = true;
                    break;
            }

            return chk;
        }

        public Panel Create(CampoTabela field, FormPrincipal form)
        {
            var panel = new Panel
            {
                Location = new System.Drawing.Point(3, Y),
                Name = "panel" + field.Id,
                Size = new System.Drawing.Size(551, 27),
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
                    Name = "Mussak - Banco Teste",
                    ConnectionString =
                        @"Server=MUSSAK-PC\SQLEXPRESS;Database=BancoTeste;Trusted_Connection=True;MultipleActiveResultSets=true"
                },
                new Connections
                {
                    Name = "Momentum Homolog",
                    ConnectionString =
                        @"server=192.168.0.192;database=GrupoKasil_Homolog;user id=homolog;password=sqlprod"
                },
                 new Connections
                {
                    Name = "GCPro Homolog",
                    ConnectionString =
                        @"server=192.168.7.12;database=GCProHomolog;user id=sqlhomolog;password=1eng@ENG"
                }
            };
            return a;
        }
    }
}

