﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class DiaChiForm : Form
    {
        public sendDiaChi send;
        public sendDiaChi send1;
        private string maDC;
        private List<DiaChi> list = null;

        public DiaChiForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
        }
        public DiaChiForm(sendDiaChi sender, DiaChi diaChi = null)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            TTP_ComboBox.DataSource = HeThong.LoadTinh_ThanhPho();

            if(diaChi == null)
            {
                txtDiaChi.Text = "Địa chỉ mới";
                maDC = null;
            }
            else
            {
                txtDiaChi.Text = "Cập nhật địa chỉ";
                this.init(diaChi);
                maDC = diaChi.maDC;
            }

            this.send = sender;
        }

        public DiaChiForm(sendDiaChi sender1, sendDiaChi sender2, DiaChi dcMacDinh, params DiaChi [] list)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            DCnhanHangForm.Visible = true;
            DCnhanHangForm.BringToFront();

            this.list = list.ToList();
            this.list.Insert(0, dcMacDinh);
            init1();
            this.send = sender1;
            this.send1 = sender2;
        }

        private Panel DrawDCPanel(DiaChi diaChi)
        {
            Panel panel = new Panel
            {
                Size = dcPanel.Size,
                Margin = dcPanel.Margin,
                BackColor = Color.White,
                Parent = listDCFLP
            };
            panel.Paint += DrawPanelBorder;

            Button button = new Button
            {
                Size = chooseButton.Size,
                Location = chooseButton.Location,
                Image = chooseButton.Image,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                
            };
            button.Click += ChonDiaChi_Click;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button);

            TextBox textBox = new TextBox
            {
                Size = moTaDCtxt.Size,
                Location = moTaDCtxt.Location,
                Cursor = Cursors.IBeam,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Text = diaChi.ToString(),
                Font = moTaDCtxt.Font,
                ReadOnly = true,
                Multiline = true,
                Parent = panel
            };
            panel.Controls.Add(textBox);


            return panel;
        }

        private void init1()
        {
            listDCFLP.Controls.Clear();
            foreach(DiaChi diaChi in list)
            {
                listDCFLP.Controls.Add(DrawDCPanel(diaChi));
            }
            listDCFLP.Controls.Add(themDCButton);
        }

        private void DrawPanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;

            using (var pen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(-1, -1, panel.Width + 2, panel.Height));
            }
        }

        private void ChonDiaChi_Click(object sender, EventArgs e)
        {
            int index = listDCFLP.Controls.IndexOf(((Control)sender).Parent);
            send1(list[index]);
            Close();
        }

        private void init(DiaChi diaChi)
        {
            hoVaTen_Box.Text = diaChi.ten;
            soDienThoai_Box.Text = diaChi.soDT;
            TTP_ComboBox.SelectedIndex = diaChi.maT_TP;

            QH_ComboBox.Enabled = true;
            QH_ComboBox.DataSource = HeThong.LoadQuan_Huyen(TTP_ComboBox.SelectedIndex);
            QH_ComboBox.SelectedIndex = diaChi.maQH % 100;

            PX_ComboBox.Enabled = true;
            PX_ComboBox.DataSource = HeThong.LoadPhuong_Xa(TTP_ComboBox.SelectedIndex, QH_ComboBox.SelectedIndex);
            PX_ComboBox.SelectedIndex = diaChi.maPX % 100;

            diaChiCuThe_Box.Enabled = true;
            diaChiCuThe_Box.Text = diaChi.diaChiCuThe;
        }

        private void troVeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HTThemDiaChi_Button_Click(object sender, EventArgs e)
        {
            if (maDC == null)
                maDC = HeThong.MaMoi("maDC");
            int maT_TP = TTP_ComboBox.SelectedIndex;
            int maQH = maT_TP * 100 + QH_ComboBox.SelectedIndex;
            int maPX = maQH * 100 + PX_ComboBox.SelectedIndex;

            DiaChi diaChi = new DiaChi(maDC, hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);

            send(diaChi);
            Close();
        }

        private void TTP_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TTP_ComboBox.SelectedIndex != 0)
            {
                QH_ComboBox.Enabled = true;
                QH_ComboBox.DataSource = HeThong.LoadQuan_Huyen(TTP_ComboBox.SelectedIndex);
            }
            else
            {
                QH_ComboBox.Enabled = false;
                QH_ComboBox.DataSource = null;
            }
        }

        private void QH_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QH_ComboBox.SelectedIndex != 0)
            {
                PX_ComboBox.Enabled = true;
                PX_ComboBox.DataSource = HeThong.LoadPhuong_Xa(TTP_ComboBox.SelectedIndex, QH_ComboBox.SelectedIndex);
            }
            else
            {
                PX_ComboBox.Enabled = false;
                PX_ComboBox.DataSource = null;
            }
        }

        private void PX_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PX_ComboBox.SelectedIndex == 0 || PX_ComboBox.Enabled == false)
            {
                diaChiCuThe_Box.Text = "";
                diaChiCuThe_Box.Enabled = false;
                HTThemDiaChi_Button.Enabled = false;
            }
            else
            {
                diaChiCuThe_Box.Enabled = true;
                HTThemDiaChi_Button.Enabled = true;
            }
        }

        private void Thoat_ESC(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HTThemDiaChi_Button_Click(sender, e);
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        public void themDiaChi(DiaChi diaChi)
        {
            list.Insert(1, diaChi);
            send(diaChi);
            init1();
        }

        private void themDCButton_Click(object sender, EventArgs e)
        {
            DiaChiForm form = new DiaChiForm(themDiaChi);
            form.ShowDialog();

        }
    }
}
