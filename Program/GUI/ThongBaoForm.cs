using Program.Properties;
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
    public partial class ThongBaoForm : Form
    {
        private Timer timer;
        public ThongBaoForm(string title)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.5;

            pictureBox1.Image = GUI_Utils.Instance.Resize(Resources.correct, pictureBox1.Size);

            titleTxt.Text = title;
            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}
