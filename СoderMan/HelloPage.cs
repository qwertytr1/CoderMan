using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СoderMan
{
    public partial class HelloPage : Form
    {
        private Timer formTimer;
        public HelloPage()
        {
            InitializeComponent();
            formTimer = new Timer();
            formTimer.Interval = 5000;
            formTimer.Tick += FormTimer_Tick;
            formTimer.Start();

        }
        private void FormTimer_Tick(object sender, EventArgs e)
        {
            RulesPages frm = new RulesPages();
            frm.Show();
            formTimer.Stop();
            this.Hide();
    
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

        }

        private void StartPageText_Click(object sender, EventArgs e)
        {

        }
    }
}
