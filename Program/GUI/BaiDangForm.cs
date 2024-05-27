using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.GUI
{
    public partial class BaiDangForm : Form
    {
        public SendBaiDang send;
        public string url;
        string maBD = null;
        string maS = null;
        public BaiDangForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public BaiDangForm(SendBaiDang send,  bool isAdd = true)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.send = send;
        }

        public void SetBaiDang(BaiDang baiDang)
        {
            url = baiDang.anhBia;
            picAnh.Image = GUI_Utils.Instance.Resize(Image.FromFile(url), picAnh.Size);
            txtTieuDe.Text = baiDang.tieuDe;
            txtGiamGia.Text = baiDang.giamGia.ToString();
            txtMoTa.Text = baiDang.moTa;
            maBD = baiDang.maBD;
            maS = baiDang.maS;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTieuDe.Text != "" && txtMoTa.Text != "" && txtGiamGia.Text != "")
            {
                this.send(new BaiDang {
                    anhBia = url,
                    maBD = maBD,
                    maS = maS,
                    tieuDe = txtTieuDe.Text,
                    giamGia = int.Parse(txtGiamGia.Text),
                    moTa = txtMoTa.Text
                }) ;
            }
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
