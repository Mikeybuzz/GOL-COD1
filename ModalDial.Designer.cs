namespace GOLStartUp
{
    partial class ModalDialog
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
            this.Okbutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ComboTimerInterval = new System.Windows.Forms.ComboBox();
            this.ComboUniverseWidth = new System.Windows.Forms.ComboBox();
            this.ComboUniverseHeight = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Okbutton
            // 
            this.Okbutton.Location = new System.Drawing.Point(155, 101);
            this.Okbutton.Name = "Okbutton";
            this.Okbutton.Size = new System.Drawing.Size(75, 23);
            this.Okbutton.TabIndex = 0;
            this.Okbutton.Text = "Ok";
            this.Okbutton.UseVisualStyleBackColor = true;
            this.Okbutton.Click += new System.EventHandler(this.Okbutton_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Location = new System.Drawing.Point(281, 101);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 1;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Timer Interval In Milliseconds";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Width of Universe in Cells";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Height of Universe in Cells";
            // 
            // ComboTimerInterval
            // 
            this.ComboTimerInterval.FormattingEnabled = true;
            this.ComboTimerInterval.Location = new System.Drawing.Point(360, 11);
            this.ComboTimerInterval.Name = "ComboTimerInterval";
            this.ComboTimerInterval.Size = new System.Drawing.Size(121, 21);
            this.ComboTimerInterval.TabIndex = 5;
            this.ComboTimerInterval.Text = "100";
            // 
            // ComboUniverseWidth
            // 
            this.ComboUniverseWidth.FormattingEnabled = true;
            this.ComboUniverseWidth.Location = new System.Drawing.Point(360, 43);
            this.ComboUniverseWidth.Name = "ComboUniverseWidth";
            this.ComboUniverseWidth.Size = new System.Drawing.Size(121, 21);
            this.ComboUniverseWidth.TabIndex = 6;
            this.ComboUniverseWidth.Text = "15";
            this.ComboUniverseWidth.SelectedIndexChanged += new System.EventHandler(this.ComboUniverseWidth_SelectedIndexChanged);
            // 
            // ComboUniverseHeight
            // 
            this.ComboUniverseHeight.FormattingEnabled = true;
            this.ComboUniverseHeight.Location = new System.Drawing.Point(360, 71);
            this.ComboUniverseHeight.Name = "ComboUniverseHeight";
            this.ComboUniverseHeight.Size = new System.Drawing.Size(121, 21);
            this.ComboUniverseHeight.TabIndex = 7;
            this.ComboUniverseHeight.Text = "15";
            // 
            // ModalDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 208);
            this.Controls.Add(this.ComboUniverseHeight);
            this.Controls.Add(this.ComboUniverseWidth);
            this.Controls.Add(this.ComboTimerInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.Okbutton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalDialog";
            this.Text = "ModalDialog";
            this.Load += new System.EventHandler(this.ModalDial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Okbutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComboTimerInterval;
        private System.Windows.Forms.ComboBox ComboUniverseWidth;
        private System.Windows.Forms.ComboBox ComboUniverseHeight;
    }
}