
namespace App_Perusahaan
{
    partial class FormLogin
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
            this.cBoxShow = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxLoginPass = new System.Windows.Forms.TextBox();
            this.tBoxLoginID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cBoxShow
            // 
            this.cBoxShow.AutoSize = true;
            this.cBoxShow.Location = new System.Drawing.Point(442, 219);
            this.cBoxShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cBoxShow.Name = "cBoxShow";
            this.cBoxShow.Size = new System.Drawing.Size(148, 24);
            this.cBoxShow.TabIndex = 13;
            this.cBoxShow.Text = "Show Password";
            this.cBoxShow.UseVisualStyleBackColor = true;
            this.cBoxShow.CheckedChanged += new System.EventHandler(this.cBoxShow_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(278, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "EATEZ";
            // 
            // tBoxLoginPass
            // 
            this.tBoxLoginPass.Location = new System.Drawing.Point(184, 218);
            this.tBoxLoginPass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tBoxLoginPass.MaxLength = 20;
            this.tBoxLoginPass.Name = "tBoxLoginPass";
            this.tBoxLoginPass.Size = new System.Drawing.Size(234, 26);
            this.tBoxLoginPass.TabIndex = 11;
            this.tBoxLoginPass.Text = "12345678";
            // 
            // tBoxLoginID
            // 
            this.tBoxLoginID.Location = new System.Drawing.Point(184, 164);
            this.tBoxLoginID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tBoxLoginID.Name = "tBoxLoginID";
            this.tBoxLoginID.Size = new System.Drawing.Size(234, 26);
            this.tBoxLoginID.TabIndex = 10;
            this.tBoxLoginID.Text = "A001";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(238, 314);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(133, 48);
            this.buttonLogin.TabIndex = 7;
            this.buttonLogin.Text = "Log In";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 474);
            this.Controls.Add(this.cBoxShow);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBoxLoginPass);
            this.Controls.Add(this.tBoxLoginID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonLogin);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormLogin";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cBoxShow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBoxLoginPass;
        private System.Windows.Forms.TextBox tBoxLoginID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonLogin;
    }
}

