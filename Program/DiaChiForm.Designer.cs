namespace Program
{
    partial class DiaChiForm
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
            this.soDTKhongHopLe_Label = new System.Windows.Forms.Label();
            this.indexOfDiaChi = new System.Windows.Forms.Label();
            this.HTThemDiaChi_Button = new System.Windows.Forms.Button();
            this.troVeButton = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.PX_ComboBox = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.QH_ComboBox = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.TTP_ComboBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.diaChiCuThe_Box = new System.Windows.Forms.TextBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.soDienThoai_Box = new System.Windows.Forms.TextBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.hoVaTen_Box = new System.Windows.Forms.TextBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.txtDiaChi = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.SuspendLayout();
            // 
            // soDTKhongHopLe_Label
            // 
            this.soDTKhongHopLe_Label.AutoSize = true;
            this.soDTKhongHopLe_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soDTKhongHopLe_Label.ForeColor = System.Drawing.Color.Red;
            this.soDTKhongHopLe_Label.Location = new System.Drawing.Point(292, 116);
            this.soDTKhongHopLe_Label.Name = "soDTKhongHopLe_Label";
            this.soDTKhongHopLe_Label.Size = new System.Drawing.Size(162, 16);
            this.soDTKhongHopLe_Label.TabIndex = 41;
            this.soDTKhongHopLe_Label.Text = "Số điện thọa không hợp lệ";
            this.soDTKhongHopLe_Label.Visible = false;
            // 
            // indexOfDiaChi
            // 
            this.indexOfDiaChi.AutoSize = true;
            this.indexOfDiaChi.Location = new System.Drawing.Point(534, -1);
            this.indexOfDiaChi.Name = "indexOfDiaChi";
            this.indexOfDiaChi.Size = new System.Drawing.Size(0, 16);
            this.indexOfDiaChi.TabIndex = 40;
            this.indexOfDiaChi.Visible = false;
            // 
            // HTThemDiaChi_Button
            // 
            this.HTThemDiaChi_Button.BackColor = System.Drawing.Color.Gainsboro;
            this.HTThemDiaChi_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.HTThemDiaChi_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HTThemDiaChi_Button.Location = new System.Drawing.Point(387, 391);
            this.HTThemDiaChi_Button.Name = "HTThemDiaChi_Button";
            this.HTThemDiaChi_Button.Size = new System.Drawing.Size(147, 41);
            this.HTThemDiaChi_Button.TabIndex = 32;
            this.HTThemDiaChi_Button.Text = "Hoàn thành";
            this.HTThemDiaChi_Button.UseVisualStyleBackColor = false;
            this.HTThemDiaChi_Button.Click += new System.EventHandler(this.HTThemDiaChi_Button_Click);
            // 
            // troVeButton
            // 
            this.troVeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.troVeButton.Location = new System.Drawing.Point(223, 391);
            this.troVeButton.Name = "troVeButton";
            this.troVeButton.Size = new System.Drawing.Size(147, 41);
            this.troVeButton.TabIndex = 33;
            this.troVeButton.Text = "Trở về";
            this.troVeButton.UseVisualStyleBackColor = true;
            this.troVeButton.Click += new System.EventHandler(this.troVeButton_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label23.Location = new System.Drawing.Point(32, 235);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(73, 16);
            this.label23.TabIndex = 38;
            this.label23.Text = "Phường/Xã";
            // 
            // PX_ComboBox
            // 
            this.PX_ComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PX_ComboBox.DropDownHeight = 200;
            this.PX_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PX_ComboBox.Enabled = false;
            this.PX_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PX_ComboBox.FormattingEnabled = true;
            this.PX_ComboBox.IntegralHeight = false;
            this.PX_ComboBox.Location = new System.Drawing.Point(23, 245);
            this.PX_ComboBox.Name = "PX_ComboBox";
            this.PX_ComboBox.Size = new System.Drawing.Size(511, 28);
            this.PX_ComboBox.TabIndex = 26;
            this.PX_ComboBox.SelectedIndexChanged += new System.EventHandler(this.PX_ComboBox_SelectedIndexChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label22.Location = new System.Drawing.Point(304, 158);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(82, 16);
            this.label22.TabIndex = 37;
            this.label22.Text = "Quận/Huyện";
            // 
            // QH_ComboBox
            // 
            this.QH_ComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.QH_ComboBox.DropDownHeight = 200;
            this.QH_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QH_ComboBox.Enabled = false;
            this.QH_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QH_ComboBox.FormattingEnabled = true;
            this.QH_ComboBox.IntegralHeight = false;
            this.QH_ComboBox.Location = new System.Drawing.Point(295, 167);
            this.QH_ComboBox.Name = "QH_ComboBox";
            this.QH_ComboBox.Size = new System.Drawing.Size(239, 28);
            this.QH_ComboBox.TabIndex = 25;
            this.QH_ComboBox.SelectedIndexChanged += new System.EventHandler(this.QH_ComboBox_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label21.Location = new System.Drawing.Point(32, 158);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 16);
            this.label21.TabIndex = 36;
            this.label21.Text = "Tỉnh/Thành phố";
            // 
            // TTP_ComboBox
            // 
            this.TTP_ComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TTP_ComboBox.DropDownHeight = 200;
            this.TTP_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TTP_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTP_ComboBox.FormattingEnabled = true;
            this.TTP_ComboBox.IntegralHeight = false;
            this.TTP_ComboBox.Location = new System.Drawing.Point(23, 167);
            this.TTP_ComboBox.Name = "TTP_ComboBox";
            this.TTP_ComboBox.Size = new System.Drawing.Size(236, 28);
            this.TTP_ComboBox.TabIndex = 24;
            this.TTP_ComboBox.Tag = "";
            this.TTP_ComboBox.SelectedIndexChanged += new System.EventHandler(this.TTP_ComboBox_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label20.Location = new System.Drawing.Point(32, 316);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(97, 18);
            this.label20.TabIndex = 35;
            this.label20.Text = "Địa chỉ cụ thể";
            // 
            // diaChiCuThe_Box
            // 
            this.diaChiCuThe_Box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.diaChiCuThe_Box.Enabled = false;
            this.diaChiCuThe_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diaChiCuThe_Box.Location = new System.Drawing.Point(35, 338);
            this.diaChiCuThe_Box.Name = "diaChiCuThe_Box";
            this.diaChiCuThe_Box.Size = new System.Drawing.Size(482, 19);
            this.diaChiCuThe_Box.TabIndex = 28;
            // 
            // pictureBox11
            // 
            this.pictureBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox11.Enabled = false;
            this.pictureBox11.Location = new System.Drawing.Point(20, 325);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(514, 48);
            this.pictureBox11.TabIndex = 34;
            this.pictureBox11.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label19.Location = new System.Drawing.Point(307, 60);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(94, 18);
            this.label19.TabIndex = 31;
            this.label19.Text = "Số điện thoại";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label18.Location = new System.Drawing.Point(32, 61);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 18);
            this.label18.TabIndex = 29;
            this.label18.Text = "Họ và tên";
            // 
            // soDienThoai_Box
            // 
            this.soDienThoai_Box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.soDienThoai_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soDienThoai_Box.Location = new System.Drawing.Point(310, 83);
            this.soDienThoai_Box.Name = "soDienThoai_Box";
            this.soDienThoai_Box.Size = new System.Drawing.Size(207, 19);
            this.soDienThoai_Box.TabIndex = 22;
            this.soDienThoai_Box.Text = "...";
            // 
            // pictureBox10
            // 
            this.pictureBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox10.Location = new System.Drawing.Point(295, 68);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(239, 48);
            this.pictureBox10.TabIndex = 27;
            this.pictureBox10.TabStop = false;
            // 
            // hoVaTen_Box
            // 
            this.hoVaTen_Box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hoVaTen_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoVaTen_Box.Location = new System.Drawing.Point(35, 82);
            this.hoVaTen_Box.Name = "hoVaTen_Box";
            this.hoVaTen_Box.Size = new System.Drawing.Size(207, 19);
            this.hoVaTen_Box.TabIndex = 21;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox9.Location = new System.Drawing.Point(20, 68);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(239, 48);
            this.pictureBox9.TabIndex = 23;
            this.pictureBox9.TabStop = false;
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.AutoSize = true;
            this.txtDiaChi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiaChi.Location = new System.Drawing.Point(14, 8);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(0, 32);
            this.txtDiaChi.TabIndex = 20;
            // 
            // DiaChiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(548, 450);
            this.Controls.Add(this.soDTKhongHopLe_Label);
            this.Controls.Add(this.indexOfDiaChi);
            this.Controls.Add(this.HTThemDiaChi_Button);
            this.Controls.Add(this.troVeButton);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.PX_ComboBox);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.QH_ComboBox);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.TTP_ComboBox);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.diaChiCuThe_Box);
            this.Controls.Add(this.pictureBox11);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.soDienThoai_Box);
            this.Controls.Add(this.pictureBox10);
            this.Controls.Add(this.hoVaTen_Box);
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.txtDiaChi);
            this.Name = "DiaChiForm";
            this.Text = "DiaChiForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label soDTKhongHopLe_Label;
        private System.Windows.Forms.Label indexOfDiaChi;
        private System.Windows.Forms.Button HTThemDiaChi_Button;
        private System.Windows.Forms.Button troVeButton;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox PX_ComboBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox QH_ComboBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox TTP_ComboBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox diaChiCuThe_Box;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox soDienThoai_Box;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.TextBox hoVaTen_Box;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label txtDiaChi;
    }
}