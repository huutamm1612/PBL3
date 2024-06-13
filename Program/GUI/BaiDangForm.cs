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

        public BaiDangForm(SendBaiDang send)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.send = send;
        }

        public void SetBaiDang(BaiDang baiDang)
        {
            url = Utils.Instance.SetPath() + baiDang.anhBia;
            picAnh.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(baiDang.anhBia), picAnh.Size);
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
                    anhBia = Utils.Instance.GetImageURL(System.Drawing.Image.FromFile(url)),
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

        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picAnh.Image = GUI_Utils.Instance.Resize(Image.FromFile(openFile.FileName), picAnh.Size);
                url = openFile.FileName;
            }
        }
    }
}
