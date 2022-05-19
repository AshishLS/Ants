using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.IO;

namespace Ants
{
    public partial class Form1 : Form
    {
        Timer timer;
        List<AntInfo> ants;
        public static Random RandomGenerator = new Random();
        //public static int IShouldInformTheseManyAnts = 10;

        public Form1()
        {
            this.GetUserDefinedVariables();
            InitializeComponent();

            this.ants = new List<AntInfo>();
            for (int i = 0; i < Form1.numberOfAnts; i++)
            {
                AntInfo ant = new AntInfo(this.btn1[i], this.home.Location);
                this.ants.Add(ant);
            }

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += onTimerTick;
            timer.Start();
        }

        private void GetUserDefinedVariables()
        {
            string message = "";
            if (File.Exists("input.txt"))
            {
                string[] lines = File.ReadAllLines("input.txt");
                Form1.formHeight = int.Parse(lines[1]);
                Form1.formWidth = int.Parse(lines[2]);
                Form1.numberOfAnts = int.Parse(lines[3]);
                Form1.AntSpeed = Double.Parse(lines[4]);

                message = String.Format(" Form Height = {0} \n Form Width = {1} \n Number Of Ants = {2} \n Speed = {3}x",
                                                lines[1], lines[2], lines[3], lines[4]);
            }
            else
            {
                message = "No input file found. Default values will be taken";
            }
            MessageBox.Show(message);
        }

        void onTimerTick(object sender, EventArgs e)
        {
            foreach (var item in this.ants)
            {
                item.Move();
                item.CheckClashWithFood(this.food);
                foreach (var otherAnt in this.ants)
                {
                    if(item.GetHashCode().Equals(otherAnt.GetHashCode())) // Don't check clash with itself.
                    {
                        continue;
                    }
                    item.CheckClashWithOtherAnt(otherAnt);
                }
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var mouseEvntArgs = e as MouseEventArgs;
            if (mouseEvntArgs != null)
            {
                // Food Button.
                this.food = new Button();
                int xval = mouseEvntArgs.X;
                int yval = mouseEvntArgs.Y;
                this.food.Location = new System.Drawing.Point(xval, yval);
                this.food.Name = "Food";
                this.food.Size = new System.Drawing.Size(30, 30);
                this.food.TabIndex = 0;
                this.food.Text = "";
                this.food.BackColor = Color.Purple;
                this.Controls.Add(this.food);
            }
        }
    }
}
