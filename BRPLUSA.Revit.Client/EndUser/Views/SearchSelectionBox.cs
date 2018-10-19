using System;
using System.Windows.Forms;

namespace BRPLUSA.Revit.Client.EndUser.Views
{
    public partial class SearchSelectionBox : Form
    {
        public string ElementName { get; private set; }
        public string ElementValue { get; private set; }

        public SearchSelectionBox()
        {
            InitializeComponent();
        }

        private void SearchForElement(object sender, EventArgs e)
        {
            ElementName = paramNameEntry.Text;
            ElementValue = paramValueEntry.Text;

            this.Close();
        }
    }
}
