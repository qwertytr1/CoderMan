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
    public partial class RulesPages : Form
    {
        public RulesPages()
        {
            InitializeComponent();
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            ChooseGame frm = new ChooseGame();
            frm.Show();
            this.Hide();
        }

        private void LevelChecker_Click(object sender, EventArgs e)
        {
            levelMenu frm = new levelMenu();
            frm.Show();
            this.Hide();
        }

        private void ResultsButton_Click(object sender, EventArgs e)
        {
            try
            {

                string pdfFilePath = @"\\Mac\Home\Desktop\курсовой\код\СoderMan\СoderMan\Assets\help\help.pdf";




                System.Diagnostics.Process.Start(pdfFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии файла PDF: {ex.Message}");
            }
        }
    }
}
