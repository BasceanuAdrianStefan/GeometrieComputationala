namespace S10
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
            this.buttonPartizioneaza = new System.Windows.Forms.Button();
            this.buttonTrianguleaza = new System.Windows.Forms.Button();
            this.buttonInchide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPartizioneaza
            // 
            this.buttonPartizioneaza.Location = new System.Drawing.Point(618, 21);
            this.buttonPartizioneaza.Name = "buttonPartizioneaza";
            this.buttonPartizioneaza.Size = new System.Drawing.Size(89, 23);
            this.buttonPartizioneaza.TabIndex = 0;
            this.buttonPartizioneaza.Text = "Partitioneaza";
            this.buttonPartizioneaza.UseVisualStyleBackColor = true;
            this.buttonPartizioneaza.Click += new System.EventHandler(this.buttonPartizioneaza_Click);
            // 
            // buttonTrianguleaza
            // 
            this.buttonTrianguleaza.Location = new System.Drawing.Point(618, 70);
            this.buttonTrianguleaza.Name = "buttonTrianguleaza";
            this.buttonTrianguleaza.Size = new System.Drawing.Size(89, 23);
            this.buttonTrianguleaza.TabIndex = 1;
            this.buttonTrianguleaza.Text = "Triangulare";
            this.buttonTrianguleaza.UseVisualStyleBackColor = true;
            this.buttonTrianguleaza.Click += new System.EventHandler(this.buttonTrianguleaza_Click);
            // 
            // buttonInchide
            // 
            this.buttonInchide.Location = new System.Drawing.Point(676, 415);
            this.buttonInchide.Name = "buttonInchide";
            this.buttonInchide.Size = new System.Drawing.Size(75, 23);
            this.buttonInchide.TabIndex = 2;
            this.buttonInchide.Text = "Inchide";
            this.buttonInchide.UseVisualStyleBackColor = true;
            this.buttonInchide.Click += new System.EventHandler(this.buttonInchide_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonInchide);
            this.Controls.Add(this.buttonTrianguleaza);
            this.Controls.Add(this.buttonPartizioneaza);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPartizioneaza;
        private System.Windows.Forms.Button buttonTrianguleaza;
        private System.Windows.Forms.Button buttonInchide;
    }
}

