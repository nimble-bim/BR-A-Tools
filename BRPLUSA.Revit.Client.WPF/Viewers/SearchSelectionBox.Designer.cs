namespace BRPLUSA.Revit.Client.WPF.Viewers
{
    partial class SearchSelectionBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.parameterName = new System.Windows.Forms.Label();
            this.parameterValue = new System.Windows.Forms.Label();
            this.paramNameEntry = new System.Windows.Forms.TextBox();
            this.paramValueEntry = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // parameterName
            // 
            this.parameterName.AutoSize = true;
            this.parameterName.Location = new System.Drawing.Point(21, 22);
            this.parameterName.Name = "parameterName";
            this.parameterName.Size = new System.Drawing.Size(86, 13);
            this.parameterName.TabIndex = 0;
            this.parameterName.Text = "Parameter Name";
            // 
            // parameterValue
            // 
            this.parameterValue.AutoSize = true;
            this.parameterValue.Location = new System.Drawing.Point(21, 55);
            this.parameterValue.Name = "parameterValue";
            this.parameterValue.Size = new System.Drawing.Size(85, 13);
            this.parameterValue.TabIndex = 0;
            this.parameterValue.Text = "Parameter Value";
            // 
            // paramNameEntry
            // 
            this.paramNameEntry.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.paramNameEntry.Location = new System.Drawing.Point(114, 22);
            this.paramNameEntry.Name = "paramNameEntry";
            this.paramNameEntry.Size = new System.Drawing.Size(200, 20);
            this.paramNameEntry.TabIndex = 1;
            this.paramNameEntry.Text = "Enter Parameter Name";
            // 
            // paramValueEntry
            // 
            this.paramValueEntry.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.paramValueEntry.Location = new System.Drawing.Point(114, 52);
            this.paramValueEntry.Name = "paramValueEntry";
            this.paramValueEntry.Size = new System.Drawing.Size(200, 20);
            this.paramValueEntry.TabIndex = 1;
            this.paramValueEntry.Text = "Enter Parameter Value";
            // 
            // buttonSearch
            // 
            this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSearch.Location = new System.Drawing.Point(241, 84);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.SearchForElement);
            // 
            // SearchSelectionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 118);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.paramValueEntry);
            this.Controls.Add(this.paramNameEntry);
            this.Controls.Add(this.parameterValue);
            this.Controls.Add(this.parameterName);
            this.Name = "SearchSelectionBox";
            this.Text = "SearchSelectionBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label parameterName;
        private System.Windows.Forms.Label parameterValue;
        private System.Windows.Forms.TextBox paramNameEntry;
        private System.Windows.Forms.TextBox paramValueEntry;
        private System.Windows.Forms.Button buttonSearch;
    }
}