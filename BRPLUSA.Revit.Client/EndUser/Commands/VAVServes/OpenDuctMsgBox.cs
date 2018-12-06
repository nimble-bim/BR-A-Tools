using System;
using System.Windows.Forms;

namespace BRPLUSA.Revit.Client.EndUser.Commands.VAVServes
{
    public partial class OpenDuctMsgBox : Form
    {
        public OpenDuctMsgBox()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void OpenDuctMsgBox_Load(object sender, EventArgs e)
        {

        }
    }
}
