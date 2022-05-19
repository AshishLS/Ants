using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
namespace Ants
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
            this.btn1 = new List<Button>();
            this.SuspendLayout();

            for (int i = 0; i < numberOfAnts; i++)
            {
                Button btn = new Button();
                this.btn1.Add(btn);
            }
            // 
            // btn1
            //
            int xval = 0;
            int yval = 0;
            foreach (var item in this.btn1)
            {
                xval = Form1.RandomGenerator.Next(0, Form1.formWidth);
                yval = Form1.RandomGenerator.Next(0, Form1.formHeight);
                item.Location = new System.Drawing.Point(xval, yval);
                item.Name = "btn1";
                item.Size = new System.Drawing.Size(15, 16);
                item.TabIndex = 0;
                item.Text = "";
                item.UseVisualStyleBackColor = true;                
            }

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.AddRange(this.btn1.ToArray());
            this.Name = "Form1";
            this.Text = "Form1";
            this.Size = new System.Drawing.Size(formWidth, formHeight);

            // Home Button
            // Food Button.
            this.home = new Button();
            xval = Form1.RandomGenerator.Next(0, Form1.formWidth);
            yval = Form1.RandomGenerator.Next(0, Form1.formHeight);
            this.home.Location = new System.Drawing.Point(xval, yval);
            this.home.Name = "Home";
            this.home.Size = new System.Drawing.Size(30, 30);
            this.home.TabIndex = 0;
            this.home.Text = "";
            this.home.BackColor = Color.Black;
            this.Controls.Add(this.home);

            // Button down event
            this.Click += new System.EventHandler(this.Form1_Click);
            this.ResumeLayout(false);
        }

        #endregion

        private List<Button> btn1;
        private Button food;
        private Button home;
        public static int formHeight = 400;
        public static int formWidth = 800;
        public static int numberOfAnts = 30;
        public static double AntSpeed = 4.0;
    }
}

