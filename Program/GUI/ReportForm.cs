using Program.BLL;
using Program.DTO;
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

    public delegate void sendLyDo(string noiDung);

    public partial class ReportForm : Form
    {
        private string noiDung = "";
        private sendLyDo send;

        public ReportForm(sendLyDo sender, string loaiBaoCao)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.send = sender;

            hoanThanhButton.Enabled = false;
            if (loaiBaoCao == "Khách hàng hủy đơn")
            {
                SetFLP(BLL_DonHang.Instance.GetAllLyDoHuyDonByKhachHang());
                hoanThanhButton.Text = "HỦY ĐƠN HÀNG";
                textBox13.Text = "Lý Do Hủy";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
            else if(loaiBaoCao == "Tố cáo bài đăng")
            {
                SetFLP(BLL_BaiDang.Instance.GetAllLyDoToCaoBaiDang());
                hoanThanhButton.Text = "Gửi";
                textBox13.Text = "Lý Do Sản Phẩm Vi phạm";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
            else if(loaiBaoCao == "Báo cáo đánh giá")
            {
                SetFLP(BLL_DanhGia.Instance.GetAllLyDoBaoCaoDanhGia());
                hoanThanhButton.Text = "Gửi";
                textBox13.Text = "Lý Do Đánh Giá Vi Phạm";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
        }

        private void SetFLP(List<string> listLyDo)
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (string s in listLyDo)
            {
                RadioButton rad = new RadioButton
                {
                    Text = s,
                    Size = new Size(flowLayoutPanel1.Width, 30),
                    Font = radioButton1.Font,
                    Margin = radioButton1.Margin,
                    Cursor = Cursors.Hand,
                    Parent = flowLayoutPanel1
                };
                rad.CheckedChanged += RadioButton_CheckedChanged;
                flowLayoutPanel1.Controls.Add(rad);
            }
            RadioButton khac = new RadioButton
            {
                Text = "Khác",
                Size = new Size(flowLayoutPanel1.Width, 30),
                Font = radioButton1.Font,
                Margin = radioButton1.Margin,
                Cursor = Cursors.Hand,
                Parent = flowLayoutPanel1
            };
            khac.CheckedChanged += RadioButton_CheckedChanged;
            flowLayoutPanel1.Controls.Add(khac);

            GUI_Utils.Instance.FitFLPHeight(flowLayoutPanel1);
            this.Height = flowLayoutPanel1.Height + 150;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton obj = sender as RadioButton;
            hoanThanhButton.Enabled = true;
            if (obj.Text.Equals("Khác"))    
            {
                flowLayoutPanel1.Controls.Add(panel1);
            }
            else if(noiDung == "Khác")
            {
                flowLayoutPanel1.Controls.Remove(panel1);
            }
            GUI_Utils.Instance.FitFLPHeight(flowLayoutPanel1);
            this.Height = flowLayoutPanel1.Height + 150;
            noiDung = obj.Text;
        }

        private void troVeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hoanThanhButton_Click(object sender, EventArgs e)
        {
            send(noiDung);
            Close();
        }
    }
}
