namespace Program.GUI
{
    partial class ReportForm
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.hoanThanhButton = new System.Windows.Forms.Button();
            this.troVeButton = new System.Windows.Forms.Button();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lyDoKhacTxt = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Controls.Add(this.radioButton1);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(56, 100);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(476, 475);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(3, 10);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(136, 28);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // hoanThanhButton
            // 
            this.hoanThanhButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.hoanThanhButton.BackColor = System.Drawing.Color.OrangeRed;
            this.hoanThanhButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hoanThanhButton.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            this.hoanThanhButton.FlatAppearance.BorderSize = 0;
            this.hoanThanhButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Tomato;
            this.hoanThanhButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.hoanThanhButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hoanThanhButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoanThanhButton.ForeColor = System.Drawing.Color.White;
            this.hoanThanhButton.Location = new System.Drawing.Point(358, 620);
            this.hoanThanhButton.Margin = new System.Windows.Forms.Padding(4);
            this.hoanThanhButton.Name = "hoanThanhButton";
            this.hoanThanhButton.Size = new System.Drawing.Size(184, 40);
            this.hoanThanhButton.TabIndex = 36;
            this.hoanThanhButton.Text = "HỦY ĐƠN HÀNG";
            this.hoanThanhButton.UseVisualStyleBackColor = false;
            this.hoanThanhButton.Click += new System.EventHandler(this.hoanThanhButton_Click);
            // 
            // troVeButton
            // 
            this.troVeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.troVeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.troVeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.troVeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.troVeButton.Location = new System.Drawing.Point(153, 620);
            this.troVeButton.Margin = new System.Windows.Forms.Padding(4);
            this.troVeButton.Name = "troVeButton";
            this.troVeButton.Size = new System.Drawing.Size(184, 40);
            this.troVeButton.TabIndex = 37;
            this.troVeButton.Text = "TRỞ VỀ";
            this.troVeButton.UseVisualStyleBackColor = true;
            this.troVeButton.Click += new System.EventHandler(this.troVeButton_Click);
            // 
            // textBox13
            // 
            this.textBox13.BackColor = System.Drawing.Color.White;
            this.textBox13.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox13.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox13.Location = new System.Drawing.Point(35, 32);
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(240, 29);
            this.textBox13.TabIndex = 75;
            this.textBox13.Text = "Lý Do Hủy";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lyDoKhacTxt);
            this.panel1.Location = new System.Drawing.Point(3, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 54);
            this.panel1.TabIndex = 1;
            // 
            // lyDoKhacTxt
            // 
            this.lyDoKhacTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lyDoKhacTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lyDoKhacTxt.Location = new System.Drawing.Point(11, 14);
            this.lyDoKhacTxt.Name = "lyDoKhacTxt";
            this.lyDoKhacTxt.Size = new System.Drawing.Size(442, 21);
            this.lyDoKhacTxt.TabIndex = 0;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(572, 682);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.hoanThanhButton);
            this.Controls.Add(this.troVeButton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ReportForm";
            this.Text = "ReportForm";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button hoanThanhButton;
        private System.Windows.Forms.Button troVeButton;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox lyDoKhacTxt;
    }
}