namespace Graphics3D
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.ver1 = new System.Windows.Forms.Label();
            this.drawLinuButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(751, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Polygon";
            // 
            // ver1
            // 
            this.ver1.AutoSize = true;
            this.ver1.Location = new System.Drawing.Point(811, 12);
            this.ver1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ver1.Name = "ver1";
            this.ver1.Size = new System.Drawing.Size(44, 17);
            this.ver1.TabIndex = 3;
            this.ver1.Text = "coord";
            // 
            // drawLinuButton
            // 
            this.drawLinuButton.Location = new System.Drawing.Point(751, 526);
            this.drawLinuButton.Name = "drawLinuButton";
            this.drawLinuButton.Size = new System.Drawing.Size(209, 35);
            this.drawLinuButton.TabIndex = 7;
            this.drawLinuButton.Text = "DRAW CUBES";
            this.drawLinuButton.UseVisualStyleBackColor = true;
            this.drawLinuButton.Click += new System.EventHandler(this.drawCoordButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(732, 549);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.form_MouseWheel);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 576);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.drawLinuButton);
            this.Controls.Add(this.ver1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label ver1;
        private System.Windows.Forms.Button drawLinuButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

