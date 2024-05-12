using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class DiaChiForm : Form
    {
        public sendDiaChi send;
        private string maDC;

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

        public DiaChiForm(params DiaChi [] list)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;


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
    }
}
