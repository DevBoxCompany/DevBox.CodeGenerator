namespace DevBox.CodeGenerator
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNomeTabela = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuscarFields = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNomeTabela
            // 
            this.txtNomeTabela.Location = new System.Drawing.Point(157, 46);
            this.txtNomeTabela.Name = "txtNomeTabela";
            this.txtNomeTabela.Size = new System.Drawing.Size(231, 20);
            this.txtNomeTabela.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nome da Tabela:";
            // 
            // btnBuscarFields
            // 
            this.btnBuscarFields.Location = new System.Drawing.Point(409, 44);
            this.btnBuscarFields.Name = "btnBuscarFields";
            this.btnBuscarFields.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarFields.TabIndex = 2;
            this.btnBuscarFields.Text = "Buscar";
            this.btnBuscarFields.UseVisualStyleBackColor = true;
            this.btnBuscarFields.Click += new System.EventHandler(this.btnBuscarFields_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 501);
            this.Controls.Add(this.btnBuscarFields);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNomeTabela);
            this.Name = "FormPrincipal";
            this.Text = "Gerador de Código";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNomeTabela;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBuscarFields;
    }
}

