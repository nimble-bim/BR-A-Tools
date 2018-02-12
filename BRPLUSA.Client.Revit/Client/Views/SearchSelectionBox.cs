using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BRPLUSA.Revit.Services;

namespace BRPLUSA.Revit.Client.Views
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
