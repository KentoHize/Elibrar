
namespace TestPlatform
{
    partial class DockableWindow1
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
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.Location = new System.Drawing.Point(392, 1);
            // 
            // FloatButton
            // 
            this.FloatButton.FlatAppearance.BorderSize = 0;
            this.FloatButton.Location = new System.Drawing.Point(364, 1);            
            // 
            // AutoHideButton
            // 
            this.AutoHideButton.FlatAppearance.BorderSize = 0;
            this.AutoHideButton.Location = new System.Drawing.Point(336, 1);
            // 
            // DockableWindow1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 385);
            this.Name = "DockableWindow1";
            this.Text = "DockableWindow1";
            this.ResumeLayout(false);

        }

        #endregion
    }
}