using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Program.DTO;

namespace Program.GUI
{
    public partial class DanhGiaForm : Form
    {
        public SendData send;

        public DanhGiaForm(SendData sender, DonHang donHang)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
        }

        private void troVeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Panel DrawDanhGiaPanel(DanhGia danhGia)
        {
            return null;
        }
    }
}
